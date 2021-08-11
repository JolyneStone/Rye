﻿using System;

namespace Rye.AspectFlare.DynamicProxy
{
    public class ReturnCaller<T> : Caller<T>
    {
        public ReturnCaller(InterceptorWrapper wrapper) : base(wrapper) { }

        public override T Call(object owner, Func<T> call, object[] parameters, string methodName)
        {
            if (_wrapper == null)
            {
                return call();
            }

            InterceptResult result;
            T returnValue = default;
            var returnType = typeof(T);
            try
            {
                result = _wrapper.CallingIntercepts(owner, parameters, returnType, methodName);
                if (result.HasResult)
                {
                    return (T)result.Result;
                }

                returnValue = call();
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
