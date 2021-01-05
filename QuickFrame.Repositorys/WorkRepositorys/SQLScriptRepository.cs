using Microsoft.EntityFrameworkCore;
using QuickFrame.Models;
using System;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 提供业务库的SQL脚本
    /// </summary>
    internal class SQLScriptRepository : Repository, ISQLScriptRepository
    {
        public SQLScriptRepository(IUnitOfWork<WorkOption> unitOfWork) : base(unitOfWork) { }
        /// <summary>
        /// 工作单元
        /// </summary>
        public IUnitOfWork Work => _unitOfWork ?? throw new ArgumentNullException($"{nameof(_unitOfWork)}已释放");
        /// <summary>
        /// 生成SQL脚本
        /// </summary>
        /// <returns></returns>
        public string GenerateCreateScript()
        {
            return Work.Context.Database.GenerateCreateScript();
        }
        /// <summary>
        /// 析构
        /// </summary>
        ~SQLScriptRepository() => Dispose();
    }
}
