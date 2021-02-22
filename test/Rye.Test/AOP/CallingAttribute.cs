using System;
using System.Diagnostics;

using Rye.AspectFlare;

namespace Rye.Test.AOP
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
