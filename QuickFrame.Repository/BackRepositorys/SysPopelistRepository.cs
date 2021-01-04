using QuickFrame.Models;

namespace QuickFrame.Repository
{
    /// <summary>
    /// 层次-权限表仓储
    /// </summary>
    internal class SysPopelistRepository : RepositoryBase<syspopelist_pl, string>, ISysPopelistRepository
    {
        public SysPopelistRepository(IUnitOfWork<BackOption> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
