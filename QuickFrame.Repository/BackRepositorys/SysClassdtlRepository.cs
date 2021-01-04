using QuickFrame.Models;

namespace QuickFrame.Repository
{
    /// <summary>
    /// 分类表仓储
    /// </summary>
    internal class SysClassdtlRepository : RepositoryBase<sysclassdtl_cd, (string, string)>, ISysClassdtlRepository
    {
        public SysClassdtlRepository(IUnitOfWork<BackOption> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
