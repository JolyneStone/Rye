using System;
using System.Diagnostics;

namespace Monica.Test.AOP
{
    [Called]
    public interface ITest
    {
        [Calling]
        void Output();
    }

    public class TestClass : ITest
    {
        [Exception]
        public void Output()
        {
            Debug.WriteLine("output ..");
        }
    }
}
