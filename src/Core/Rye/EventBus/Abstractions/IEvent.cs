namespace Rye.EventBus.Abstractions
{
    public interface IEvent
    {
        public long EventId { get; set; }
    }

    //public interface IEvent<in T>: IEvent where T : class, new()
    //{
    //}
}
