﻿using QuickFrame.Models;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 用户表仓储
    /// </summary>
    internal class SysUserlistRepository : RepositoryBase<sysuserlist_ul, string>, ISysUserlistRepository
    {
        public SysUserlistRepository(IUnitOfWork<BackOption> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
