using System;
using System.Diagnostics;

namespace Rye.Util
{
    public class IdGenerator
    {
        public const long Twepoch = 1609430400000L; // 起始时间戳 2021-01-01 00:00:00

        private const int WorkerIdBits = 5;
        private const int DatacenterIdBits = 5;
        private const int SequenceBits = 12;
        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);

        private const int WorkerIdShift = SequenceBits;
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
        public const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);

        private static IdGenerator _idGenerator;

        private readonly object _iLock = new object();
        private static readonly object _sLock = new object();
        private long _lastTimestamp = -1L;

        public IdGenerator(long workerId, long datacenterId, long sequence = 0L)
        {
            WorkerId = workerId;
            DatacenterId = datacenterId;
            Sequence = sequence;

            if (workerId > MaxWorkerId || workerId < 0)
                throw new ArgumentException($"workerId需大于0并且小于{MaxWorkerId}");

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
                throw new ArgumentException($"datacenterId需大于0并且小于{MaxDatacenterId}");
        }

        public long WorkerId { get; protected set; }
        public long DatacenterId { get; protected set; }

        public long Sequence { get; internal set; }

        /// <summary>
        /// 注册雪花算法的基础参数
        /// </summary>
        /// <param name="workerId">工作机器Id</param>
        /// <param name="datacenterId">数据中心Id</param>
        /// <param name="sequence">序列号</param>
        public static void Register(long workerId, long datacenterId, long sequence = 0L)
        {
            if (_idGenerator != null)
            {
                return;
            }

            lock (_sLock)
            {
                if (_idGenerator != null)
                {
                    return;
                }

                _idGenerator = new IdGenerator(workerId, datacenterId, sequence);
            }
        }

        public static IdGenerator Instance
        {
            get
            {
                if (_idGenerator != null)
                {
                    return _idGenerator;
                }

                lock (_sLock)
                {
                    if (_idGenerator != null)
                    {
                        return _idGenerator;
                    }

                    var random = new Random();

                    if (!int.TryParse(Environment.GetEnvironmentVariable("RYE_WORKERID", EnvironmentVariableTarget.Machine), out var workerId))
                    {
                        workerId = random.Next((int)MaxWorkerId);
                    }
                    else
                    {
                        workerId = workerId ^ Process.GetCurrentProcess().Id;
                    }

                    if (!int.TryParse(Environment.GetEnvironmentVariable("RYE_DATACENTERID", EnvironmentVariableTarget.Machine), out var datacenterId))
                    {
                        datacenterId = random.Next((int)MaxDatacenterId);
                    }

                    return _idGenerator = new IdGenerator(workerId, datacenterId);
                }
            }
        }

        public virtual long NextId()
        {
            lock (_iLock)
            {
                var timestamp = TimeGen();

                if (timestamp < _lastTimestamp)
                    throw new Exception(
                        $"时钟向后移动，{_lastTimestamp - timestamp}毫秒内无法生成Id");

                if (_lastTimestamp == timestamp)
                {
                    Sequence = (Sequence + 1) & SequenceMask;
                    if (Sequence == 0) timestamp = TilNextMillis(_lastTimestamp);
                }
                else
                {
                    Sequence = 0;
                }

                _lastTimestamp = timestamp;
                var id = ((timestamp - Twepoch) << TimestampLeftShift) |
                         (DatacenterId << DatacenterIdShift) |
                         (WorkerId << WorkerIdShift) | Sequence;

                return id;
            }
        }

        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp) timestamp = TimeGen();
            return timestamp;
        }

        protected virtual long TimeGen()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
