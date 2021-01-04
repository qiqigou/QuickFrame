using QuickFrame.Models;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 用户仓储
    /// </summary>
    internal class UserinfoRepository : RepositoryBase<userinfo_us, int>, IUserinfoRepository
    {
        public UserinfoRepository(IUnitOfWork<WorkOption> unitOfWork) : base(unitOfWork)
        {
        }
    }
}