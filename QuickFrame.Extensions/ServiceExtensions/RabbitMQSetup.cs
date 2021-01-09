using Microsoft.Extensions.Logging;
using QuickFrame.Common;
using QuickFrame.EventBus;
using RabbitMQ.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Rabbitmq配置
    /// </summary>
    public static class RabbitMQSetup
    {
        /// <summary>
        /// 注入Rabbitmq配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appconfig"></param>
        public static IServiceCollection AddRabbitMQSetup(this IServiceCollection services, AppConfig appconfig)
        {
            if (!appconfig.RabbitMQ.Enabled) return services;
            return services.AddSingleton<IRabbitMQPersistentConnection>(provder =>
            {
                var logger = provder.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory
                {
                    HostName = appconfig.RabbitMQ.HostName,
                    DispatchConsumersAsync = true
                };
                factory.UserName = appconfig.RabbitMQ.UserName;
                factory.Password = appconfig.RabbitMQ.PassWork;
                return new RabbitMQPersistentConnection(factory, logger, appconfig.RabbitMQ.RetryCount);
            });
        }
    }
}
