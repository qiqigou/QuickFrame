namespace QuickFrame.EventBus
{
    /// <summary>
    /// 事件总线
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="e">事件模型</param>
        void Publish(IntegrationEvent e);
        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="TEvent">事件模型</typeparam>
        /// <typeparam name="THandle">事件处理程序</typeparam>
        void Subscribe<TEvent, THandle>()
            where TEvent : IntegrationEvent
            where THandle : IIntegrationEventHandle<TEvent>;
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="TEvent">事件模型</typeparam>
        /// <typeparam name="THandle">事件处理程序</typeparam>
        void Unsubscribe<TEvent, THandle>()
            where TEvent : IntegrationEvent
            where THandle : IIntegrationEventHandle<TEvent>;
    }
}
