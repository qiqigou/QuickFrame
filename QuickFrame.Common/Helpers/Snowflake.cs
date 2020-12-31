using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// 雪花算法(生成唯一ID)
    /// </summary>
    public class Snowflake
    {
        //基准时间
        private static readonly long StartStmp = 1288834974657L;
        //private const long START_STMP = 1480166465631L;
        /*每一部分占用的位数*/
        //机器标识位数
        const int MachineIdBits = 5;
        //数据标志位数
        const int DatacenterIdBits = 5;
        //序列号识位数
        const int SequenceBits = 12;

        /* 每一部分的最大值*/
        //机器ID最大值
        const long MaxMachineNum = -1L ^ (-1L << MachineIdBits);
        //数据标志ID最大值
        const long MaxDatacenterNum = -1L ^ (-1L << DatacenterIdBits);
        //序列号ID最大值
        private const long MaxSequenceNum = -1L ^ (-1L << SequenceBits);

        /*每一部分向左的位移*/
        //机器ID偏左移12位
        private const int MachineShift = SequenceBits;
        //数据ID偏左移17位
        private const int DatacenterIdShift = SequenceBits + MachineIdBits;
        //时间毫秒左移22位
        public const int TimestampLeftShift = SequenceBits + MachineIdBits + DatacenterIdBits;

        private long _sequence = 0L;//序列号
        private long _lastTimestamp = -1L;//上一次时间戳
        public long MachineId { get; protected set; }//机器标识
        public long DatacenterId { get; protected set; }//数据中心
        //public long Sequence = 0L;//序列号
        //{
        //    get { return _sequence; }
        //    internal set { _sequence = value; }
        //}

        private readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private readonly object _lock = new object();
        public Snowflake(long machineId, long datacenterId)
        {
            // 如果超出范围就抛出异常
            if (machineId > MaxMachineNum || machineId < 0)
            {
                throw new ArgumentException($"machineId 必须大于0，MaxMachineNum： {MaxMachineNum}");
            }

            if (datacenterId > MaxDatacenterNum || datacenterId < 0)
            {
                throw new ArgumentException($"datacenterId必须大于0，且不能大于MaxDatacenterNum： {MaxDatacenterNum}");
            }

            //先检验再赋值
            MachineId = machineId;
            DatacenterId = datacenterId;
            //_sequence = sequence;
        }

        public long NextId()
        {
            lock (_lock)
            {
                var timestamp = TimeGen();
                if (timestamp < _lastTimestamp)
                {
                    throw new Exception($"时间戳必须大于上一次生成ID的时间戳.  拒绝为{_lastTimestamp - timestamp}毫秒生成id");
                }

                //如果上次生成时间和当前时间相同,在同一毫秒内
                if (_lastTimestamp == timestamp)
                {
                    //sequence自增，和sequenceMask相与一下，去掉高位
                    _sequence = (_sequence + 1) & MaxSequenceNum;
                    //判断是否溢出,也就是每毫秒内超过1024，当为1024时，与sequenceMask相与，sequence就等于0
                    if (_sequence == 0L)
                    {
                        //等待到下一毫秒
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    //如果和上次生成时间不同,重置sequence，就是下一毫秒开始，sequence计数重新从0开始累加,
                    //为了保证尾数随机性更大一些,最后一位可以设置一个随机数
                    _sequence = 0L;//new Random().Next(10);
                }

                _lastTimestamp = timestamp;
                return ((timestamp - StartStmp) << TimestampLeftShift) | (DatacenterId << DatacenterIdShift) | (MachineId << MachineShift) | _sequence;
            }
        }
        /// <summary>
        /// 防止产生的时间比之前的时间还要小（由于NTP回拨等问题）,保持增量的趋势.
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }
        /// <summary>
        /// 获取当前的时间戳
        /// </summary>
        /// <returns></returns>
        protected virtual long TimeGen()
        {
            //return TimeExtensions.CurrentTimeMillis();
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}
