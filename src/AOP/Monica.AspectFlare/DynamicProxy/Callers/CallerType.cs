namespace Monica.AspectFlare.DynamicProxy
{
    internal enum CallerType
    {
        Ctor = 0,
        Void = 1,
        Return = 2,
        Task = 3,
        TaskOfT = 4,
        ValueTaskOfT = 5
    }
}
