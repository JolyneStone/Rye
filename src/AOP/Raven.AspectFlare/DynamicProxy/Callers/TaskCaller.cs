using System;
using System.Threading.Tasks;

namespace Raven.AspectFlare.DynamicProxy
{
    public class TaskCaller<T> : Caller<Task<T>>
    {
        public TaskCaller(InterceptorWrapper wrapper) : base(wrapper) { }

        public override async Task<T> Call(object owner, Func<Task<T>> call, object[] parameters)
        {
            if (_wrapper == null)
            {
                return await call();
            }

            InterceptResult result;
            T returnValue = default(T);
            try
            {
                result = _wrapper.CallingIntercepts(owner, parameters);
                if (result.HasResult)
                {
                    return (T)result.Result;
                }

                returnValue = await call();

                result = _wrapper.CalledIntercepts(owner, parameters, returnValue);
                if (result.HasResult)
                {
                    return (T)result.Result;
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                result = _wrapper.ExceptionIntercept(owner, parameters, returnValue, ex);
                if (result.HasResult)
                {
                    return (T)result.Result;
                }

                throw ex;
            }
        }
    }
}
