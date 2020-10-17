using System;

namespace Monica.AspectFlare
{
    public class ExceptionInterceptContext
    {
        public object Owner { get; set; }
        public object[] Parameters { get; set; }
        public object ReturnValue { get; set; }
        public Exception Exception { get; set; }
        public bool HasHandled { get; set; }
        public object Result { get; set; }
        public Type ReturnType { get; internal set; }
        public string MethodName { get; internal set; }
    }
}
