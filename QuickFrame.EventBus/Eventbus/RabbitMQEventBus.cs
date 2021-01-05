using Autofac;
using Microsoft.Extensions.Logging;
using Polly;
using QuickFrame.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuickFrame.EventBus
{
    /// <summary>
    /// 基于RabbitMQ的事件总线
    /// </summary>
    [SingletonInjection]
    public class RabbitMQEventBus : IEventBus, IDisposable
    {
        private const string BROKER_NAME = "event_bus";
        private const string AUTOFAC_SCOPE_NAME = "event_bus";
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IEventBusSubscriptionsManage _subsManager;
        private readonly ILifetimeScope _autofac;
        private readonly int _retryCount;
        private readonly string _queueName;
        private IModel? _consumerChannel;

        /// <summary>
        /// RabbitMQ事件总线
        /// </summary>
        /// <param name="logger">日志</param>
        /// <param name="persistentConnection">RabbitMQ持久连接</param>
        /// <param name="autofac">autofac容器</param>
        /// <param name="subsManager">事件总线订阅管理器</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="retryCount">重试次数</param>
        public RabbitMQEventBus(
            ILogger<RabbitMQEventBus> logger,
            IRabbitMQPersistentConnection persistentConnection,
            ILifetimeScope autofac,
            IEventBusSubscriptionsManage subsManager,
            int retryCount, string queueName)
        {
            _logger = logger;
            _persistentConnection = persistentConnection;
            _subsManager = subsManager;
            _queueName = queueName;
            _consumerChannel = CreateConsumerChannel();
            _autofac = autofac;
            _retryCount = retryCount;
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }
        /// <summary>
        /// 移除订阅后触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventName"></param>
        private void SubsManager_OnEventRemoved(object? sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            using var channel = _persistentConnection.CreateModel();
            channel.QueueUnbind(_queueName, BROKER_NAME, eventName);
            if (_subsManager.IsEmpty)
            {
                _consumerChannel?.Close();
            }
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="e">事件模型</param>
        public void Publish(IntegrationEvent e)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            var eventName = e.GetType().Name;
            using var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(BROKER_NAME, "direct");
            var body = JsonSerializer.SerializeToUtf8Bytes(e);

            Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { })
            .Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;
                channel.BasicPublish(BROKER_NAME, eventName, true, properties, body);
            });
        }
        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="TEvent">事件模型</typeparam>
        /// <typeparam name="THandle">事件处理器</typeparam>
        public void Subscribe<TEvent, THandle>()
            where TEvent : IntegrationEvent
            where THandle : IIntegrationEventHandle<TEvent>
        {
            var eventName = _subsManager.GetEventKey<TEvent>();
            var containsKey = _subsManager.HasSubscriptionsForEvent<TEvent>();
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }
                using var channel = _persistentConnection.CreateModel();
                channel.QueueBind(_queueName, BROKER_NAME, eventName);
            }
            _subsManager.AddSubscription<TEvent, THandle>();
            StartBasicConsume();
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="TEvent">事件模型</typeparam>
        /// <typeparam name="THandle">事件处理器</typeparam>
        public void Unsubscribe<TEvent, THandle>()
            where TEvent : IntegrationEvent
            where THandle : IIntegrationEventHandle<TEvent>
        {
            _subsManager.RemoveSubscription<TEvent, THandle>();
        }
        /// <summary>
        /// 开始基本消费
        /// </summary>
        private void StartBasicConsume()
        {
            if (_consumerChannel != default)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                consumer.Received += Consumer_Received;
                _consumerChannel.BasicConsume(_queueName, false, consumer);
            }
        }
        /// <summary>
        /// 消费者接受到
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }
                if (_subsManager.HasSubscriptionsForEvent(eventName))
                {
                    using var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME);
                    var handles = _subsManager.GetHandlesForEvent(eventName);
                    foreach (var handle in handles)
                    {
                        var handler = scope.ResolveOptional(handle);
                        if (handler == default) continue;
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        if (eventType == default) continue;
                        var integrationEvent = JsonSerializer.Deserialize(message, eventType);
                        var concreteType = typeof(IIntegrationEventHandle<>).MakeGenericType(eventType);
                        await Task.Yield();
                        await Task.FromResult(concreteType.GetMethod(nameof(IIntegrationEventHandle<IntegrationEvent>.Handle))?.Invoke(handler, new object?[] { integrationEvent }));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }
            _consumerChannel?.BasicAck(eventArgs.DeliveryTag, false);
        }
        /// <summary>
        /// 创造消费通道
        /// </summary>
        /// <returns></returns>
        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(BROKER_NAME, "direct");
            channel.QueueDeclare(_queueName, true, false, false, null);
            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel?.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };
            return channel;
        }
        /// <summary>
        /// 析构
        /// </summary>
        ~RabbitMQEventBus() => Dispose();
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 实现释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _consumerChannel?.Dispose();
            }
            _consumerChannel = default;
            _subsManager.Clear();
        }
    }
}
