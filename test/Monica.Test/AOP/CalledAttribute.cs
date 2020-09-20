using System;
using System.Diagnostics;

using Monica.AspectFlare;

namespace Monica.Test.AOP
{
    public class CalledAttribute : CalledInterceptAttribute
    {
        public override void Called(CalledInterceptContext calledInterceptorContext)
        {
            Debug.WriteLine("Called in " + calledInterceptorContext.Owner.ToString());
        }
    }
}