namespace QuickFrame.Common
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheConfig
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType Type { get; set; }
        /// <summary>
        /// Redis配置
        /// </summary>
        public RedisConfig Redis { get; set; } = new RedisConfig();
    }
    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
    /// <summary>
    /// 缓存类型
    /// </summary>
    public enum CacheType : byte
    {
        Memory = 0,
        Redis = 1,
    }
}
