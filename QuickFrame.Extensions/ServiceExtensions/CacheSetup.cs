using CSRedis;
using Microsoft.Extensions.Configuration;
using QuickFrame.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public static class CacheSetup
    {
        /// <summary>
        /// 注入缓存配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCacheSetup(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheconfig = configuration.Get<CacheConfig>();
            if (cacheconfig.Type == CacheType.Redis)
            {
                //Redis缓存
                RedisHelper.Initialization(new CSRedisClient(cacheconfig.Redis.ConnectionString));
                services.AddSingleton<ICache, RedisCache>();
            }
            else
            {
                //内存缓存
                services.AddMemoryCache();
                services.AddSingleton<ICache, MemorysCache>();
            }
            return services;
        }
    }
}
