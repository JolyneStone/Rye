using Rye.Cache;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Rye.Test.AOP
{
    [Called]
    public interface ITest
    {
        [Calling]
        Task<string> Output(int i);
    }

    public class TestClass : ITest
    {
        //private int i = 0;
        [Exception]
        //[Cache("OutputTest")]
        public Task<string> Output(int i)
        {
            Debug.WriteLine("output ..");
            //throw new Exception("exception test");
            return Task.FromResult("qwe" + i++);
        }
    }
}
