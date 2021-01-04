using Microsoft.EntityFrameworkCore;
using QuickFrame.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace QuickFrame.Repository
{
    /// <summary>
    /// 工作单元(用于区分DbContext)
    /// </summary>
    /// <typeparam name="TOption">DbContext类别</typeparam>
    public interface IUnitOfWork<out TOption> : IUnitOfWork where TOption : IContextOption { }
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// 上下文
        /// </summary>
        internal DbContext Context { get; }
        /// <summary>
        /// 禁用自动保存
        /// </summary>
        bool DisableAutoSave { get; set; }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="level">隔离级别</param>
        /// <returns></returns>
        IUnitOfWorkTransaction BeginTransaction(IsolationLevel level = IsolationLevel.ReadCommitted);
        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// 保存更改(异步)
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
        /// <summary>
        /// 自动保存
        /// </summary>
        /// <returns></returns>
        internal int AutoSaveChanges();
        /// <summary>
        /// 自动保存(异步)
        /// </summary>
        /// <returns></returns>
        internal Task<int> AutoSaveChangesAsync();
    }
}
