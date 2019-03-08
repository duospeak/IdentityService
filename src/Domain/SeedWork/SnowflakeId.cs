using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.SeedWork
{
    public class SnowflakeId
    {
        public const long Twepoch = 1288834974657L;

        private const int WorkerIdBits = 5;

        private const int DatacenterIdBits = 5;

        private const int SequenceBits = 12;

        private const long MaxWorkerId = 31L;

        private const long MaxDatacenterId = 31L;

        private const int WorkerIdShift = 12;

        private const int DatacenterIdShift = 17;

        public const int TimestampLeftShift = 22;

        private const long SequenceMask = 4095L;

        private static SnowflakeId _snowflakeId;

        private readonly object _lock = new object();

        private static readonly object SLock = new object();

        private long _lastTimestamp = -1L;

        public long WorkerId
        {
            get;
            protected set;
        }

        public long DatacenterId
        {
            get;
            protected set;
        }

        public long Sequence
        {
            get;
            internal set;
        }

        public SnowflakeId(long workerId, long datacenterId, long sequence = 0L)
        {
            WorkerId = workerId;
            DatacenterId = datacenterId;
            Sequence = sequence;
            if (workerId > 31 || workerId < 0)
            {
                throw new ArgumentException($"worker Id can't be greater than {31L} or less than 0");
            }
            if (datacenterId > 31 || datacenterId < 0)
            {
                throw new ArgumentException($"datacenter Id can't be greater than {31L} or less than 0");
            }
        }

        public static SnowflakeId Default()
        {
            lock (SLock)
            {
                if (_snowflakeId != null)
                {
                    return _snowflakeId;
                }
                Random random = new Random();
                int num = random.Next(31);
                int num2 = random.Next(31);
                return _snowflakeId = new SnowflakeId(num, num2, 0L);
            }
        }

        public virtual long NextId()
        {
            lock (_lock)
            {
                long num = TimeGen();
                if (num < _lastTimestamp)
                {
                    throw new Exception($"InvalidSystemClock: Clock moved backwards, Refusing to generate id for {_lastTimestamp - num} milliseconds");
                }
                if (_lastTimestamp == num)
                {
                    Sequence = ((Sequence + 1) & 0xFFF);
                    if (Sequence == 0L)
                    {
                        num = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    Sequence = 0L;
                }
                _lastTimestamp = num;
                return (num - 1288834974657L << 22) | (DatacenterId << 17) | (WorkerId << 12) | Sequence;
            }
        }

        protected virtual long TilNextMillis(long lastTimestamp)
        {
            long num;
            for (num = TimeGen(); num <= lastTimestamp; num = TimeGen())
            {
            }
            return num;
        }

        protected virtual long TimeGen()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
