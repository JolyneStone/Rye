namespace Monica.AspectFlare
{
    public interface IExceptionInterceptor : IInterceptor
    {
        void Exception(ExceptionInterceptContext exceptionInterceptorContext);
    }
}
