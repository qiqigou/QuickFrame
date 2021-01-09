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
        /// <param name="appconfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdWorkerSetup(this IServiceCollection services, AppConfig appconfig)
        {
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
