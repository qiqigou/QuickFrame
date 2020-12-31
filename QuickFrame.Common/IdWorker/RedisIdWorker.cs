using CSRedis;

namespace QuickFrame.Common
{
    /// <summary>
    /// Redis实现获取唯一ID接口
    /// </summary>
    public class RedisIdWorker : IIdWorker
    {
        private readonly CSRedisClient _redisClient;

        public RedisIdWorker(CSRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public long GetId64()
        {
            return _redisClient.IncrBy("zxsccoreapi:uuid", 1);
        }

        public string GetIdString()
        {
            return GetId64().ToString();
        }
    }
}
