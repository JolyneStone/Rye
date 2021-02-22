using System;
using System.Diagnostics;

using Rye.AspectFlare;

namespace Rye.Test.AOP
{
    public class CalledAttribute : CalledInterceptAttribute
    {
        public override void Called(CalledInterceptContext calledInterceptorContext)
        {
            Debug.WriteLine("Called in " + calledInterceptorContext.Owner.ToString());
        }
    }
}