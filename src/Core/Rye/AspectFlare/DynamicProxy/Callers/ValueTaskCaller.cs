using System;
using System.Threading.Tasks;

namespace Rye.AspectFlare.DynamicProxy
{
    public class ValueTaskCaller<T> : Caller<ValueTask<T>>
    {
        public ValueTaskCaller(InterceptorWrapper wrapper) : base(wrapper) { }

        public override async ValueTask<T> Call(object owner, Func<ValueTask<T>> call, object[] parameters, string methodName)
        {
            if (_wrapper == null)
            {
                return await call();
            }

            InterceptResult result;
            T returnValue = default(T);
            var returnType = typeof(T);
            try
            {
                result = _wrapper.CallingIntercepts(owner, parameters, returnType, methodName);
                if (result.HasResult)
                {
                    return (T)result.Result;
                }

                returnValue = await call();

                result = _wrapper.CalledIntercepts(owner, parameters, returnValue, returnType, methodName);
                if (result.HasResult)
                {
                    return (T)result.Result;
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                result = _wrapper.ExceptionIntercept(owner, parameters, returnValue, ex, returnType, methodName);
                if (result.HasResult)
                {
                    return (T)result.Result;
                }

                throw;
            }
        }
    }
}
