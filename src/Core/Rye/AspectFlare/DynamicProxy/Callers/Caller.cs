using System;

namespace Rye.AspectFlare.DynamicProxy
{
    public abstract class CallerBase
    {
        protected InterceptorWrapper _wrapper { get; }

        protected CallerBase(InterceptorWrapper wrapper)
        {
            _wrapper = wrapper;
        }
    }
    public abstract class Caller : CallerBase
    {
        protected Caller(InterceptorWrapper wrapper) : base(wrapper) { }

        public abstract void Call(object owner, Action call, object[] parameters, string methodName);
    }

    public abstract class Caller<T> : CallerBase
    {
        protected Caller(InterceptorWrapper wrapper) : base(wrapper) { }

        public abstract T Call(object owner, Func<T> call, object[] parameters, string methodName);
    }
}
