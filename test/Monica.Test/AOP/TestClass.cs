using Monica.Cache;

using System;
using System.Diagnostics;

namespace Monica.Test.AOP
{
    [Called]
    public interface ITest
    {
        //[Calling]
        string Output(int i);
    }

    public class TestClass : ITest
    {
        //private int i = 0;
        [Exception]
        [Cache("OutputTest")]
        public string Output(int i)
        {
            Debug.WriteLine("output ..");
            //throw new Exception("exception test");
            return "qwe" + i++;
        }
    }
}
