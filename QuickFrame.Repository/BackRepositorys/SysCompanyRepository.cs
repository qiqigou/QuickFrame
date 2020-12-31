using QuickFrame.Model;

namespace QuickFrame.Repository
{
    /// <summary>
    /// 公司仓储
    /// </summary>
    internal class SysCompanyRepository : RepositoryBase<syscompany_scy, string>, ISysCompanyRepository
    {
        public SysCompanyRepository(IUnitOfWork<BackOption> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
