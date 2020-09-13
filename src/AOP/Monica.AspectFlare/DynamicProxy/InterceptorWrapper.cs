using System;
using System.Collections.Generic;

namespace Monica.AspectFlare.DynamicProxy
{
    public class InterceptorWrapper
    {
        public List<ICallingInterceptor> CallingInterceptors { get; set; }
        public List<ICalledInterceptor> CalledInterceptors { get; set; }
        public IExceptionInterceptor ExceptionInterceptor { get; set; }

        public InterceptResult CallingIntercepts(object owner, object[] parameters)
        {
            if (owner == null)
            {
                throw new System.ArgumentNullException(nameof(owner));
            }

            if (CallingInterceptors == null || CallingInterceptors.Count == 0)
            {
                return new InterceptResult
                {
                    HasResult = false
                };
            }

            var context = new CallingInterceptContext
            {
                Owner = owner,
                Parameters = parameters,
                HasResult = false
            };

            foreach (var callingInterceptor in CallingInterceptors)
            {
                callingInterceptor.Calling(context);
                if (context.HasResult)
                {
                    return new InterceptResult
                    {
                        HasResult = true,
                        Result = context.Result
                    };
                }
            }

            return new InterceptResult
            {
                HasResult = false
            };
        }

        public InterceptResult CalledIntercepts(object owner, object[] parameters, object returnValue)
        {
            if (owner == null)
            {
                throw new System.ArgumentNullException(nameof(owner));
            }

            if (CalledInterceptors == null || CalledInterceptors.Count == 0)
            {
                return new InterceptResult
                {
                    HasResult = false
                };
            }

            var context = new CalledInterceptContext
            {
                Owner = owner,
                Parameters = parameters,
                ReturnValue = returnValue,
                HasResult = false
            };

            foreach (var calledInterceptor in CalledInterceptors)
            {
                calledInterceptor.Called(context);
                if (context.HasResult)
                {
                    return new InterceptResult
                    {
                        HasResult = false,
                        Result = context.Result
                    };
                }
            }

            return new InterceptResult
            {
                HasResult = false
            };
        }

        public InterceptResult ExceptionIntercept(object owner, object[] parameters, object returnValue, Exception exception)
        {
            if (owner == null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (ExceptionInterceptor == null)
            {
                return new InterceptResult
                {
                    HasResult = false
                };
            }

            var context = new ExceptionInterceptContext
            {
                Owner = owner,
                Parameters = parameters,
                ReturnValue = returnValue,
                Exception = exception,
                HasHandled = false
            };

            ExceptionInterceptor.Exception(context);
            if (context.HasHandled)
            {
                return new InterceptResult
                {
                    HasResult = true,
                    Result = context.Result
                };
            }

            return new InterceptResult
            {
                HasResult = false
            };
        }
    }
}
