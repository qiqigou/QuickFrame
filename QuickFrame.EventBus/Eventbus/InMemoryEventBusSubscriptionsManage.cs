﻿//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace QuickFrame.EventBus
//{
//    /// <summary>
//    /// 事件总线订阅管理器(基于内存)
//    /// </summary>
//    public class InMemoryEventBusSubscriptionsManage : IEventBusSubscriptionsManage
//    {
//        private readonly Dictionary<string, HashSet<Type>> _handlers;
//        private readonly HashSet<Type> _eventTypes;
//        /// <summary>
//        /// 移除订阅后触发事件
//        /// </summary>
//        public event EventHandler<string>? OnEventRemoved;

//        public InMemoryEventBusSubscriptionsManage()
//        {
//            _handlers = new Dictionary<string, HashSet<Type>>(10);
//            _eventTypes = new HashSet<Type>(10);
//        }
//        /// <summary>
//        /// 是否是空订阅
//        /// </summary>
//        public bool IsEmpty => !_handlers.Keys.Any();
//        /// <summary>
//        /// 清空订阅
//        /// </summary>
//        public void Clear() => _handlers.Clear();
//        /// <summary>
//        /// 添加订阅
//        /// </summary>
//        /// <typeparam name="TEvent">事件模型</typeparam>
//        /// <typeparam name="THandle">事件处理程序</typeparam>
//        public void AddSubscription<TEvent, THandle>()
//            where TEvent : IntegrationEvent
//            where THandle : IIntegrationEventHandle<TEvent>
//        {
//            var eventName = GetEventKey<TEvent>();
//            var handleType = typeof(THandle);
//            if (!_handlers.ContainsKey(eventName))
//            {
//                _handlers.Add(eventName, new HashSet<Type>(10));
//            }
//            _handlers[eventName].Add(handleType);
//            _eventTypes.Add(typeof(TEvent));
//        }
//        /// <summary>
//        /// 移除订阅
//        /// </summary>
//        /// <typeparam name="TEvent"></typeparam>
//        /// <typeparam name="THandle"></typeparam>
//        public void RemoveSubscription<TEvent, THandle>()
//            where TEvent : IntegrationEvent
//            where THandle : IIntegrationEventHandle<TEvent>
//        {
//            var handlerToRemove = FindSubscriptionToRemove<TEvent, THandle>();
//            var eventName = GetEventKey<TEvent>();
//            var handleType = typeof(THandle);
//            _handlers[eventName].Remove(handleType);
//            if (!_handlers[eventName].Any())
//            {
//                _handlers.Remove(eventName);
//                var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
//                if (eventType != null)
//                {
//                    _eventTypes.Remove(eventType);
//                }
//                RaiseOnEventRemoved(eventName);
//            }

//        }

//        private void DoRemoveHandler(string eventName, SubscriptionInfo? subsToRemove)
//        {

//        }

//        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

//        private void RaiseOnEventRemoved(string eventName)
//        {
//            var handler = OnEventRemoved;
//            handler?.Invoke(this, eventName);
//        }

//        /// <summary>
//        /// 查询订阅并移除
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <typeparam name="TH"></typeparam>
//        /// <returns></returns>
//        private SubscriptionInfo? FindSubscriptionToRemove<T, TH>()
//             where T : IntegrationEvent
//             where TH : IIntegrationEventHandle<T>
//        {
//            var eventName = GetEventKey<T>();
//            return DoFindSubscriptionToRemove(eventName, typeof(TH));
//        }

//        private SubscriptionInfo? DoFindSubscriptionToRemove(string eventName, Type handlerType)
//        {
//            if (!HasSubscriptionsForEvent(eventName))
//            {
//                return default;
//            }

//            return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);

//        }

//        public bool HasSubscriptionsForEvent<T>()
//            where T : IntegrationEvent
//        {
//            var key = GetEventKey<T>();
//            return HasSubscriptionsForEvent(key);
//        }
//        /// <summary>
//        /// 是否含有事件的订阅
//        /// </summary>
//        /// <param name="eventName"></param>
//        /// <returns></returns>
//        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);
//        /// <summary>
//        /// 根据事件名获取事件模型
//        /// </summary>
//        /// <param name="eventName"></param>
//        /// <returns></returns>
//        public Type? GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);
//        /// <summary>
//        /// 根据事件模型获取事件名
//        /// </summary>
//        /// <typeparam name="TEvent"></typeparam>
//        /// <returns></returns>
//        public string GetEventKey<TEvent>() => typeof(TEvent).Name;
//    }

//}
