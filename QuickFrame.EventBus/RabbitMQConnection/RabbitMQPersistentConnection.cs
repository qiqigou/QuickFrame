using System;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polly;
using QuickFrame.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace QuickFrame.EventBus
{
    /// <summary>
    /// RabbitMQ持久连接
    /// </summary>
    [SingletonInjection]
    public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly object sync_locker = new object();
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMQPersistentConnection> _logger;
        private readonly int _retryCount = 5;
        private IConnection? _connection;

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, ILogger<RabbitMQPersistentConnection> logger)
            : this(connectionFactory, logger, 5) { }

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, ILogger<RabbitMQPersistentConnection> logger, int retryCount)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _retryCount = retryCount;
        }
        /// <summary>
        /// 连接
        /// </summary>
        protected IConnection Connection => _connection ?? throw new ArgumentNullException("连接已经释放或未连接");
        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnected => _connection?.IsOpen ?? false;
        /// <summary>
        /// 创建通信模型
        /// </summary>
        /// <returns></returns>
        public IModel CreateModel() => Connection.IsOpen ? Connection.CreateModel() : throw new ArgumentException("连接已关闭");
        /// <summary>
        /// 尝试连接
        /// </summary>
        /// <returns></returns>
        public bool TryConnect()
        {
            lock (sync_locker)
            {
                Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { })
                .Execute(() => _connection = _connectionFactory.CreateConnection());

                if (IsConnected)
                {
                    Connection.ConnectionShutdown += OnConnectionShutdown;
                    Connection.CallbackException += OnCallbackException;
                    Connection.ConnectionBlocked += OnConnectionBlocked;
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 连接被阻断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_connection != default)
            {
                _logger.LogWarning("连接被阻断,正在尝试重新建立连接");
                TryConnect();
            }
        }
        /// <summary>
        /// 连接出现异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_connection != default)
            {
                _logger.LogWarning("连接出现异常,正在尝试重新建立连接");
                TryConnect();
            }
        }
        /// <summary>
        /// 连接被关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="reason"></param>
        private void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
        {
            if (_connection != default)
            {
                _logger.LogWarning("连接被关闭,正在尝试重新建立连接");
                TryConnect();
            }
        }
        /// <summary>
        /// 析构
        /// </summary>
        ~RabbitMQPersistentConnection() => Dispose();
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
                _connection?.Dispose();
            }
            _connection = default;
        }
    }
}