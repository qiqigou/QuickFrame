using Microsoft.EntityFrameworkCore;
using QuickFrame.Models;
using System;
using System.Threading.Tasks;

namespace QuickFrame.Repository
{
    /// <summary>
    /// 提供业务库的SQL脚本
    /// </summary>
    internal class SQLScriptRepository : ISQLScriptRepository
    {
        private IUnitOfWork? _unitOfWork;

        public SQLScriptRepository(IUnitOfWork<WorkOption> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
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
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 释放(异步)
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 实现释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork?.Dispose();
            }
            _unitOfWork = default;
        }
        /// <summary>
        /// 实现异步释放资源
        /// </summary>
        /// <returns></returns>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_unitOfWork != null)
            {
                await _unitOfWork.DisposeAsync().ConfigureAwait(false);
            }
            _unitOfWork = default;
        }
    }
}
