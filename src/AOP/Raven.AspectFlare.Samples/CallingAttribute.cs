using System;
using Raven.AspectFlare;

namespace Simples
{
    public class CallingAttribute : CallingInterceptAttribute
    {
        public override void Calling(CallingInterceptContext callingInterceptorContext)
        {
            Console.WriteLine("Calling in " + callingInterceptorContext.Owner.ToString());
        }
    }
}
