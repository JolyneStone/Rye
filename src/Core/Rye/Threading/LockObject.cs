using System;
using System.Threading;

namespace Rye.Threading
{
    public sealed class LockObject
    {
        private SemaphoreSlim _waiterLock = new SemaphoreSlim(0, 1);
        private int _waiters = 0;

        public void Enter()
        {
            if (Interlocked.Increment(ref _waiters) == 1)
            {
                return;
            }

            _waiterLock.Wait();
        }

        public void Exit()
        {
            if(Interlocked.Decrement(ref _waiters) == 0)
            {
                return;
            }

            _waiterLock.Release();
        }
    }
}
