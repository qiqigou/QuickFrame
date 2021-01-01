using Microsoft.EntityFrameworkCore;
using QuickFrame.Common;
using QuickFrame.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace QuickFrame.Repository
{
    /// <summary>
    /// 视图仓储抽象(只读仓储)
    /// </summary>
    internal abstract class QueryRepository : IQueryRepository
    {
        private IUnitOfWork? _unitOfWork;
        private static CancellationTokenSource TokenSource => TaskCancelOption.DbTask;

        public QueryRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 工作单元
        /// </summary>
        public IUnitOfWork Work => _unitOfWork ?? throw new ArgumentNullException($"{nameof(_unitOfWork)}已释放");
        /// <summary>
        /// 查询(未跟踪)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Select<TEntity>() where TEntity : ViewEntity, new()
            => Work.Context.Set<TEntity>().AsNoTracking();
        /// <summary>
        /// 是否有一项满足条件(条件lambda树)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>>? predicate = default) where TEntity : ViewEntity, new()
            => predicate == default ? Select<TEntity>().AnyAsync(TokenSource.Token) : Select<TEntity>().AnyAsync(predicate, TokenSource.Token);
        /// <summary>
        /// 是否有一项满足条件(条件lambda树)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual bool Any<TEntity>(Expression<Func<TEntity, bool>>? predicate = default) where TEntity : ViewEntity, new()
            => predicate == default ? Select<TEntity>().Any() : Select<TEntity>().Any(predicate);
        /// <summary>
        /// 计数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>>? predicate = default) where TEntity : ViewEntity, new()
            => predicate == default ? Select<TEntity>().CountAsync(TokenSource.Token) : Select<TEntity>().CountAsync(predicate, TokenSource.Token);
        /// <summary>
        /// 计数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Count<TEntity>(Expression<Func<TEntity, bool>>? predicate = default) where TEntity : ViewEntity, new()
            => predicate == default ? Select<TEntity>().Count() : Select<TEntity>().Count(predicate);
        /// <summary>
        /// 析构
        /// </summary>
        ~QueryRepository() => Dispose();
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
    /// <summary>
    /// 后端库视图仓储
    /// </summary>
    internal class BackQueryRepository : QueryRepository, IQueryRepository<BackOption>
    {
        public BackQueryRepository(IUnitOfWork<BackOption> unitOfWork) : base(unitOfWork) { }
    }
    /// <summary>
    /// 业务库视图仓储
    /// </summary>
    internal class WorkQueryRepository : QueryRepository, IQueryRepository<WorkOption>
    {
        public WorkQueryRepository(IUnitOfWork<WorkOption> unitOfWork) : base(unitOfWork) { }
    }
}
