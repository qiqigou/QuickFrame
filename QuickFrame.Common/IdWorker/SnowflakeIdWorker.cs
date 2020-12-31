using Microsoft.Extensions.Options;

namespace QuickFrame.Common
{
    /// <summary>
    /// 雪花算法实现获取唯一ID接口
    /// </summary>
    public class SnowflakeIdWorker : IIdWorker
    {
        private readonly AppConfig _appConfig;
        private readonly Snowflake _idWorker;

        public SnowflakeIdWorker(IOptions<AppConfig> options)
        {
            _appConfig = options.Value;
            _idWorker = new Snowflake(_appConfig.MachineId, _appConfig.DatacenterId);
        }

        public long GetId64()
        {
            return _idWorker.NextId();
        }

        public string GetIdString()
        {
            return GetId64().ToString();
        }
    }
}
