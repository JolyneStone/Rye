using System;
using System.Collections.Generic;

namespace Rye.Notify
{
    public interface IServiceBroker<TNotification> where TNotification : INotification
    {
        event NotificationSuccessDelegate<TNotification> OnNotificationSucceeded;
        event NotificationFailureDelegate<TNotification> OnNotificationFailed;

        IEnumerable<TNotification> TakeMany();
        bool IsCompleted { get; }

        void RaiseNotificationSucceeded(TNotification notification);
        void RaiseNotificationFailed(TNotification notification, AggregateException ex);
        void Notify(TNotification notification);
        void Start();
        void Stop(bool immediately = false);
    }
}
