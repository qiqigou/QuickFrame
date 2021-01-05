using System;

namespace QuickFrame.EventBus
{
    /// <summary>
    /// 事件总线订阅管理器
    /// </summary>
    public interface IEventBusSubscriptionsManage
    {
        /// <summary>
        /// 移除订阅后触发事件
        /// </summary>
        event EventHandler<string> OnEventRemoved;
        /// <summary>
        /// 是否是空订阅
        /// </summary>
        bool IsEmpty { get; }
        /// <summary>
        /// 添加订阅
        /// </summary>
        /// <typeparam name="TEvent">事件模型</typeparam>
        /// <typeparam name="THandle">事件处理程序</typeparam>
        void AddSubscription<TEvent, THandle>()
            where TEvent : IntegrationEvent
            where THandle : IIntegrationEventHandle<TEvent>;
        /// <summary>
        /// 移除订阅
        /// </summary>
        /// <typeparam name="TEvent">事件模型</typeparam>
        /// <typeparam name="THandle">事件处理程序</typeparam>
        void RemoveSubscription<TEvent, THandle>()
             where THandle : IIntegrationEventHandle<TEvent>
             where TEvent : IntegrationEvent;
        /// <summary>
        /// 是否含有事件的订阅
        /// </summary>
        /// <typeparam name="TEvent">事件模型</typeparam>
        /// <returns></returns>
        bool HasSubscriptionsForEvent<TEvent>() 
            where TEvent : IntegrationEvent;
        /// <summary>
        /// 根据事件名获取事件模型
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        Type? GetEventTypeByName(string eventName);
        /// <summary>
        /// 根据事件模型获取事件名
        /// </summary>
        /// <typeparam name="TEvent">事件模型</typeparam>
        /// <returns></returns>
        string GetEventKey<TEvent>();
        /// <summary>
        /// 清空订阅
        /// </summary>
        void Clear();
    }

}
