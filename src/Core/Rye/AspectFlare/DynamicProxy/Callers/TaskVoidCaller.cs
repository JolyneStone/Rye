using System;
using System.Threading.Tasks;

namespace Rye.AspectFlare.DynamicProxy
{
    public class TaskCaller : Caller<Task>
    {
        public TaskCaller(InterceptorWrapper wrapper) : base(wrapper) { }

        public override async Task Call(object owner, Func<Task> call, object[] parameters, string methodName)
        {
            if (_wrapper == null)
            {
                await call();
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

                await call();

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

                throw;
            }
        }
    }
}
