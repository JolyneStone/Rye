using System;

namespace Raven.AspectFlare.DynamicProxy
{
    public class ReturnCaller<T> : Caller<T>
    {
        public ReturnCaller(InterceptorWrapper wrapper) : base(wrapper) { }

        public override T Call(object owner, Func<T> call, object[] parameters)
        {
            if (_wrapper == null)
            {
                return call();
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

                returnValue = call();
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
