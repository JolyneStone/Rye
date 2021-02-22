namespace Rye.Notify
{
    public interface IServiceConnectionFactory<TNotification> where TNotification : INotification
    {
        IServiceConnection<TNotification> Create();
    }
}
