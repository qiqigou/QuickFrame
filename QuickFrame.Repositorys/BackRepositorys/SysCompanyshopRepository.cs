﻿using QuickFrame.Models;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 专卖店仓储
    /// </summary>
    internal class SysCompanyshopRepository : RepositoryBase<syscompanyshop_scs, (string, string)>, ISysCompanyshopRepository
    {
        public SysCompanyshopRepository(IUnitOfWork<BackOption> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
