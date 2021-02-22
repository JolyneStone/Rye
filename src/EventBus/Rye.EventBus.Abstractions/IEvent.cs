namespace Rye.EventBus.Abstractions
{
    public interface IEvent<T> where T : class, new()
    {
    }
}
