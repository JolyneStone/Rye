using System;
using System.Diagnostics;

using Monica.AspectFlare;

namespace Monica.Test.AOP
{
    public class CallingAttribute : CallingInterceptAttribute
    {
        public override void Calling(CallingInterceptContext callingInterceptorContext)
        {
            Debug.WriteLine("Calling in " + callingInterceptorContext.Owner.ToString());
            //callingInterceptorContext.HasResult = true;
            //callingInterceptorContext.Result = "test";
        }
    }
}
