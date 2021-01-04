using QuickFrame.Models;

namespace QuickFrame.Services
{
    /// <summary>
    /// 商场表服务
    /// </summary>
    public interface ISyscompanybakService : IService, IHandle<SysCompanybakInput, SysCompanybakUpdInput, (int, string)>, IQuery<v_syscompanybak, (int, string)>
    {
    }
}
