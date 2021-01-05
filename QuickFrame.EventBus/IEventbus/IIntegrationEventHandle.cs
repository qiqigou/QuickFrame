using System.Threading.Tasks;

namespace QuickFrame.EventBus
{
    /// <summary>
    /// 集成事件处理程序
    /// </summary>
    /// <typeparam name="TIntegrationEvent"></typeparam>
    public interface IIntegrationEventHandle<in TIntegrationEvent>
       where TIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Handle(TIntegrationEvent model);
    }
}
