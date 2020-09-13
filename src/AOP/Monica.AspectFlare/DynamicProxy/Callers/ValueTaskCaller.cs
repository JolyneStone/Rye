using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monica.AspectFlare.DynamicProxy
{
    public class ValueTaskCaller<T> : Caller<ValueTask<T>>
    {
        public ValueTaskCaller(InterceptorWrapper wrapper) : base(wrapper) { }

        public override async ValueTask<T> Call(object owner, Func<ValueTask<T>> call, object[] parameters)
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
