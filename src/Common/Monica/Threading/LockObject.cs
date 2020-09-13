using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Monica.Threading
{
    public sealed class LockObject
    {
        private object _sync = new object();
        private int _taken = 0;

        public void Enter()
        {
            if (Interlocked.Exchange(ref _taken, 1) == 0)
            {
                return;
            }

            Monitor.Enter(_sync);
            Volatile.Write(ref _taken, 2);
        }

        public void Exit()
        {
            if(_taken == 2)
            {
                Monitor.Exit(_sync);
            }

            Volatile.Write(ref _taken, 0);
        }
    }
}
