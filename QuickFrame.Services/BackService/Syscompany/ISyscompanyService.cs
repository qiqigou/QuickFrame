using QuickFrame.Models;

namespace QuickFrame.Services
{
    /// <summary>
    /// 公司表服务
    /// </summary>
    public interface ISysCompanyService : IService, IHandle<SysCompanyInput, SysCompanyUpdInput, string>, IQuery<v_syscompany, string>
    {

    }
}
