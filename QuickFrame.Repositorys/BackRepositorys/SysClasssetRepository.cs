using QuickFrame.Models;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 分类基础表仓储
    /// </summary>
    internal class SysClasssetRepository : RepositoryBase<sysclassset_cs, int>, ISysClasssetRepository
    {
        public SysClasssetRepository(IUnitOfWork<BackOption> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
