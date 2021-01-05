using RabbitMQ.Client;
using System;

namespace QuickFrame.EventBus
{
    /// <summary>
    /// RabbitMQ持久连接
    /// </summary>
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        /// <summary>
        /// 是否连接
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// 尝试连接
        /// </summary>
        /// <returns></returns>
        bool TryConnect();
        /// <summary>
        /// 创建通信模型
        /// </summary>
        /// <returns></returns>
        IModel CreateModel();
    }
}
