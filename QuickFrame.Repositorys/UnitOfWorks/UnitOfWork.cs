using Microsoft.EntityFrameworkCore;
using QuickFrame.Common;
using QuickFrame.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 工作单元抽象
    /// </summary>
    internal abstract class UnitOfWork : IUnitOfWork
    {
        private DbContext? _dbContext;

        /// <summary>
        /// 工作单元
        /// </summary>
        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// 上下文
        /// </summary>
        protected DbContext CurrentContext => _dbContext ?? throw new ArgumentNullException($"{nameof(_dbContext)}已释放");
        /// <summary>
        /// 仓储层内部访问上下文
        /// </summary>
        public DbContext Context => CurrentContext;
        /// <summary>
        /// 禁用自动保存
        /// </summary>
        public bool DisableAutoSave { get; set; }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="level">隔离级别</param>
        /// <returns></returns>
        public IUnitOfWorkTransaction BeginTransaction(IsolationLevel level = IsolationLevel.ReadCommitted)
            => new UnitOfWorkTransaction(CurrentContext.Database.BeginTransaction(level));
        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        public int SaveChanges() => CurrentContext.SaveChanges();
        /// <summary>
        /// 保存更改(异步)
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync() => CurrentContext.SaveChangesAsync(TaskCancelOption.DbTask.Token);
        /// <summary>
        /// 自动保存
        /// </summary>
        /// <returns></returns>
        int IUnitOfWork.AutoSaveChanges() => DisableAutoSave ? 0 : SaveChanges();
        /// <summary>
        /// 自动保存(异步)
        /// </summary>
        /// <returns></returns>
        async Task<int> IUnitOfWork.AutoSaveChangesAsync() => DisableAutoSave ? 0 : await SaveChangesAsync();
        /// <summary>
        /// 析构
        /// </summary>
        ~UnitOfWork() => Dispose();
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
                _dbContext?.Dispose();
            }
            _dbContext = default;
        }
        /// <summary>
        /// 实现异步释放资源
        /// </summary>
        /// <returns></returns>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_dbContext != default)
            {
                await _dbContext.DisposeAsync().ConfigureAwait(false);
            }
            _dbContext = default;
        }
    }
    /// <summary>
    /// 后台库工作单元
    /// </summary>
    [ScopeInjection]
    internal class BackUnitOfWork : UnitOfWork, IUnitOfWork<BackOption>
    {
        public BackUnitOfWork(BackDbContext dbContext) : base(dbContext) { }
    }
    /// <summary>
    /// 业务库工作单元
    /// </summary>
    [ScopeInjection]
    internal class WorkUnitOfWork : UnitOfWork, IUnitOfWork<WorkOption>
    {
        public WorkUnitOfWork(WorkDbContext dbContext) : base(dbContext) { }
    }
}
