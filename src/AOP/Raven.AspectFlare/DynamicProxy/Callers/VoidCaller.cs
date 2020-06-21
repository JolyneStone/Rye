using System;

namespace Raven.AspectFlare.DynamicProxy
{
    public sealed class VoidCaller : Caller
    {
        public VoidCaller(InterceptorWrapper wrapper) : base(wrapper) {}

        public override void Call(object owner, Action call, object[] parameters)
        {
            if (_wrapper == null)
            {
                call();
                return;
            }

            InterceptResult result;
            try
            {
                result = _wrapper.CallingIntercepts(owner, parameters);
                if (result.HasResult)
                {
                    return;
                }

                call();

                result = _wrapper.CalledIntercepts(owner, parameters, null);
                if (result.HasResult)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                result = _wrapper.ExceptionIntercept(owner, parameters, null, ex);
                if (result.HasResult)
                {
                    return;
                }

                throw ex;
            }
        }
    }
}
