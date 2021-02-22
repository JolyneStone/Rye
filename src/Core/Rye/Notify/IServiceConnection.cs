using System;
using System.Threading.Tasks;

namespace Rye.Notify
{
    public delegate void NotificationSuccessDelegate<TNotification>(TNotification notification) where TNotification : INotification;
    public delegate void NotificationFailureDelegate<TNotification>(TNotification notification, AggregateException exception) where TNotification : INotification;

    public interface IServiceConnection<TNotification> where TNotification : INotification
    {
        Task Send(TNotification notification);
    }
}
