using System;
using System.Threading.Tasks;

namespace Raven.AspectFlare.DynamicProxy
{
    public class TaskCaller : Caller<Task>
    {
        public TaskCaller(InterceptorWrapper wrapper) : base(wrapper) { }

        public override async Task Call(object owner, Func<Task> call, object[] parameters)
        {
            if (_wrapper == null)
            {
                await call();
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

                await call();

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
