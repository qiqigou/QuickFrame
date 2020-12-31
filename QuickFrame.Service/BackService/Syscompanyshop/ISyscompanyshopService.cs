using QuickFrame.Model;

namespace QuickFrame.Service
{
    /// <summary>
    /// 专卖店服务
    /// </summary>
    public interface ISysCompanyshopService : IService, IHandle<SysCompanyshopInput, SysCompanyshopUpdInput, (string, string)>, IQuery<v_syscompanyshop, (string, string)>
    {

    }
}
