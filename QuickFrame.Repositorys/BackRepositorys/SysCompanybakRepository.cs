﻿using QuickFrame.Models;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 商场表仓储
    /// </summary>
    internal class SysCompanybakRepository : RepositoryBase<syscompanybak_scb, (int, string)>, ISysCompanybakRepository
    {
        public SysCompanybakRepository(IUnitOfWork<BackOption> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
