﻿using QuickFrame.Models;

namespace QuickFrame.Repositorys
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
