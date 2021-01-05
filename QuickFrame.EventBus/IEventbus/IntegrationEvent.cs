using System;

namespace QuickFrame.EventBus
{
    /// <summary>
    /// 集成事件模型
    /// </summary>
    public class IntegrationEvent
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public Guid Id { get; init; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; init; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }
    }
}
