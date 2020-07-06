using System;

namespace Simples
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
            Console.WriteLine("output ..");
        }
    }
}
