using System;

namespace Rye.AspectFlare.DynamicProxy
{
    public sealed class VoidCaller : Caller
    {
        public VoidCaller(InterceptorWrapper wrapper) : base(wrapper) {}

        public override void Call(object owner, Action call, object[] parameters, string methodName)
        {
            if (_wrapper == null)
            {
                call();
                return;
            }

            InterceptResult result;
            try
            {
                result = _wrapper.CallingIntercepts(owner, parameters, null, methodName);
                if (result.HasResult)
                {
                    return;
                }

                call();

                result = _wrapper.CalledIntercepts(owner, parameters, null, null, methodName);
                if (result.HasResult)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                result = _wrapper.ExceptionIntercept(owner, parameters, null, ex, null, methodName);
                if (result.HasResult)
                {
                    return;
                }

#pragma warning disable CA2200 // 再次引发以保留堆栈详细信息
                throw ex;
#pragma warning restore CA2200 // 再次引发以保留堆栈详细信息
            }
        }
    }
}
