using Microsoft.Extensions.Configuration;
using QuickFrame.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 唯一ID服务配置
    /// </summary>
    public static class IdWorkerSetup
    {
        /// <summary>
        /// 注入唯一ID服务配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdWorkerSetup(this IServiceCollection services, IConfiguration configuration)
        {
            var appconfig = configuration.Get<AppConfig>();
            if (appconfig.IdWorkerProvid == IdWorkerProvidType.Redis)
            {
                services.AddSingleton<IIdWorker, RedisIdWorker>();
            }
            else
            {
                services.AddSingleton<IIdWorker, SnowflakeIdWorker>();
            }
            return services;
        }
    }
}
