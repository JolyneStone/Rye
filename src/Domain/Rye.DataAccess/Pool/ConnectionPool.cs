using Microsoft.Extensions.ObjectPool;

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Rye.DataAccess.Pool
{
    public class ConnectionPool : ObjectPool<Connector>
    {
        /// <summary>
        /// 监视器计时器
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// 连接池最大容量
        /// </summary>
        private int _maximumRetained;

        private readonly ObjectWrapper[] _items;
        private readonly IPooledObjectPolicy<Connector> _policy;
        private protected Connector _firstItem;

        public ConnectionPool(IPooledObjectPolicy<Connector> policy)
            : this(policy, Environment.ProcessorCount * 2)
        {
        }

        public ConnectionPool(IPooledObjectPolicy<Connector> policy, int maximumRetained)
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _items = new ObjectWrapper[maximumRetained - 1];
            _maximumRetained = maximumRetained;
            _timer = new Timer(TimerCallback, null, 0, 10 * 60 * 1000);
        }

        public override Connector Get()
        {
            var item = _firstItem;
            if (item == null || item.Used || Interlocked.CompareExchange(ref _firstItem, null, item) != item)
            {
                var items = _items;
                for (var i = 0; i < items.Length; i++)
                {
                    item = items[i].Element;
                    if (item != null && !item.Used && Interlocked.CompareExchange(ref items[i].Element, null, item) == item)
                    {
                        return item;
                    }
                }

                item = Create();
            }
            item.Used = true;
            return item;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private Connector Create() => _policy.Create();

        /// <inheritdoc />
        public override void Return(Connector obj)
        {
            if (_policy.Return(obj))
            {
                if (_firstItem != null || Interlocked.CompareExchange(ref _firstItem, obj, null) != null)
                {
                    var items = _items;
                    for (var i = 0; i < items.Length && Interlocked.CompareExchange(ref items[i].Element, obj, null) != null; ++i)
                    {
                    }
                }
            }
        }

        void TimerCallback(object sender)
        {
            Connector item;
            int realCount = 0;
            int unUseCount = 0;
            var items = _items;
            for (var i = 0; i < items.Length; i++)
            {
                item = items[i].Element;
                if (item != null)
                {
                    realCount++;
                    if (item.Used)
                    {
                        unUseCount++;
                    }
                }
            }

            if (realCount / unUseCount >= 2) // 当缓冲池里有一半的连接都处于未使用状态时，释放未使用的连接
            {
                item = _firstItem;
                if (item != null && !item.Used)
                {
                    item.Dispose();
                    Interlocked.Exchange(ref _firstItem, null);
                }
                for (var i = 0; i < items.Length; i++)
                {
                    item = items[i].Element;
                    if (item != null && !item.Used && Interlocked.CompareExchange(ref items[i].Element, null, item) == item)
                    {
                        item.Dispose();
                        Interlocked.Exchange(ref items[i].Element, null);
                    }
                }
            }
        }

        private protected struct ObjectWrapper
        {
            public Connector Element;
        }
    }
}
