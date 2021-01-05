using System;
using System.Threading.Tasks;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    internal abstract class Repository : IRepository
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        protected IUnitOfWork? _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
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