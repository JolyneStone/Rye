using Microsoft.Extensions.Logging;

using Rye.Logger;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rye.Notify
{
    public class ServiceBroker<TNotification> : IServiceBroker<TNotification> where TNotification : INotification
    {
        private readonly ILogger<ServiceBroker<INotification>> _logger;

        static ServiceBroker()
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.Expect100Continue = false;
        }

        public ServiceBroker(IServiceConnectionFactory<TNotification> connectionFactory,
            ILogger<ServiceBroker<INotification>> logger)
        {
            ServiceConnectionFactory = connectionFactory;

            _lockWorkers = new object();
            _workers = new List<ServiceWorker<TNotification>>();
            _running = false;

            _notifications = new BlockingCollection<TNotification>();
            ScaleSize = 1;

            _logger = logger;
        }

        public event NotificationSuccessDelegate<TNotification> OnNotificationSucceeded;
        public event NotificationFailureDelegate<TNotification> OnNotificationFailed;

        public int ScaleSize { get; private set; }

        public IServiceConnectionFactory<TNotification> ServiceConnectionFactory { get; set; }

        private BlockingCollection<TNotification> _notifications;
        private List<ServiceWorker<TNotification>> _workers;
        private object _lockWorkers;
        private bool _running;

        public virtual void Notify(TNotification notification)
        {
            _notifications.Add(notification);
        }

        public IEnumerable<TNotification> TakeMany()
        {
            return _notifications.GetConsumingEnumerable();
        }

        public bool IsCompleted
        {
            get { return _notifications.IsCompleted; }
        }

        public void Start()
        {
            if (_running)
                return;

            _running = true;
            ChangeScale(ScaleSize);
        }

        public void Stop(bool immediately = false)
        {
            if (!_running)
                throw new OperationCanceledException("ServiceBroker has already been signaled to Stop");

            _running = false;

            _notifications.CompleteAdding();

            lock (_lockWorkers)
            {
                // Kill all workers right away
                if (immediately)
                    _workers.ForEach(sw => sw.Cancel());

                var all = (from sw in _workers
                           select sw.WorkerTask).ToArray();

                _logger.LogInformation("Stopping: Waiting on Tasks");

                Task.WaitAll(all);

                _logger.LogInformation("Stopping: Done Waiting on Tasks");

                _workers.Clear();
            }
        }

        public void ChangeScale(int newScaleSize)
        {
            if (newScaleSize <= 0)
                throw new ArgumentOutOfRangeException("newScaleSize", "Must be Greater than Zero");

            ScaleSize = newScaleSize;

            if (!_running)
                return;

            lock (_lockWorkers)
            {

                // Scale down
                while (_workers.Count > ScaleSize)
                {
                    _workers[0].Cancel();
                    _workers.RemoveAt(0);
                }

                // Scale up
                while (_workers.Count < ScaleSize)
                {
                    var worker = new ServiceWorker<TNotification>(this, ServiceConnectionFactory.Create(), _logger);
                    _workers.Add(worker);
                    worker.Start();
                }

                _logger.LogDebug("ServiceBroker", "Scaled Changed to: " + _workers.Count);
            }
        }

        public void RaiseNotificationSucceeded(TNotification notification)
        {
            var evt = OnNotificationSucceeded;
            if (evt != null)
                evt(notification);
        }

        public void RaiseNotificationFailed(TNotification notification, AggregateException exception)
        {
            var evt = OnNotificationFailed;
            if (evt != null)
                evt(notification, exception);
        }
    }

    public class ServiceWorker<TNotification> where TNotification : INotification
    {
        private readonly ILogger _logger;

        public ServiceWorker(IServiceBroker<TNotification> broker,
            IServiceConnection<TNotification> connection,
            ILogger logger)
        {
            Broker = broker;
            Connection = connection;

            CancelTokenSource = new CancellationTokenSource();

            _logger = logger;
        }

        public IServiceBroker<TNotification> Broker { get; private set; }

        public IServiceConnection<TNotification> Connection { get; private set; }

        public CancellationTokenSource CancelTokenSource { get; private set; }

        public Task WorkerTask { get; private set; }

        public void Start()
        {
            WorkerTask = Task.Factory.StartNew(async ()=>
            {
                while (!CancelTokenSource.IsCancellationRequested && !Broker.IsCompleted)
                {

                    try
                    {

                        var toSend = new List<Task>();
                        foreach (var n in Broker.TakeMany())
                        {
                            var t = Connection.Send(n);
                            // Keep the continuation
                            var cont = t.ContinueWith(ct =>
                            {
                                var cn = n;
                                var ex = t.Exception;

                                if (ex == null)
                                    Broker.RaiseNotificationSucceeded(cn);
                                else
                                    Broker.RaiseNotificationFailed(cn, ex);
                            });

                            // Let's wait for the continuation not the task itself
                            toSend.Add(cont);
                        }

                        if (toSend.Count <= 0)
                            continue;

                        try
                        {

                            _logger.LogInformation(string.Format("Waiting on all tasks {0}", toSend.Count()));

                            await Task.WhenAll(toSend).ConfigureAwait(false);

                            _logger.LogInformation("All Tasks Finished");

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(string.Format("Waiting on all tasks Failed: {0}", ex));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(string.Format("Broker.Take: {0}", ex));
                    }
                }

                if (CancelTokenSource.IsCancellationRequested)
                    _logger.LogInformation("Cancellation was requested");
                if (Broker.IsCompleted)
                    _logger.LogInformation("Broker IsCompleted");

            }, CancelTokenSource.Token, TaskCreationOptions.LongRunning | TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();

            WorkerTask.ContinueWith(t =>
            {
                var ex = t.Exception;
                if (ex != null)
                    _logger.LogError(string.Format("ServiceWorker.WorkerTask Error: {0}", ex));
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public void Cancel()
        {
            CancelTokenSource.Cancel();
        }
    }
}
