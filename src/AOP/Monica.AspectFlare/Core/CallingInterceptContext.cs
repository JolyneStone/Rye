using System;

namespace Monica.AspectFlare
{
    public class CallingInterceptContext
    {
        public object Owner { get; set; }
        public object[] Parameters { get; set; }
        public bool HasResult { get; set; }
        public object Result { get; set; }
        public Type ReturnType { get; internal set; }
        public string MethodName { get; internal set; }
    }
}
