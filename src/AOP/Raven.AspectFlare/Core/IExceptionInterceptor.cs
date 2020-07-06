namespace Raven.AspectFlare
{
    public interface IExceptionInterceptor : IInterceptor
    {
        void Exception(ExceptionInterceptContext exceptionInterceptorContext);
    }
}
