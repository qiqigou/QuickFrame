using Microsoft.EntityFrameworkCore;
using QuickFrame.Common;
using QuickFrame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace QuickFrame.Repository
{
    /// <summary>
    /// 表仓储抽象
    /// </summary>
    public abstract class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey>
        where TEntity : TableEntity, new()
        where TKey : notnull
    {
        private static string[]? _keys;
        private IUnitOfWork? _unitOfWork;
        protected DbSet<TEntity> Set;

        protected RepositoryBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Set = _unitOfWork.Context.Set<TEntity>();
        }
        /// <summary>
        /// 超时机制
        /// </summary>
        protected static CancellationTokenSource TokenSource => TaskCancelOption.DbTask;
        /// <summary>
        /// 工作单元
        /// </summary>
        public IUnitOfWork Work => _unitOfWork ?? throw new ArgumentNullException($"{nameof(_unitOfWork)}已释放");
        /// <summary>
        /// 查询
        /// </summary>
        /// <value></value>
        public virtual IQueryable<TEntity> Select => Set.AsQueryable();
        /// <summary>
        /// 主键
        /// </summary>
        public string[] Keys => _keys ??= Work.Context.Model.GetEntityTypes().Where(x => x.ClrType == typeof(TEntity)).First().FindPrimaryKey().Properties.Select(x => x.Name).ToArray();
        /// <summary>
        /// 包含(异步)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<bool> ContainsAsync(TEntity entity) => Set.ContainsAsync(entity, TokenSource.Token);
        /// <summary>
        /// 包含
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Contains(TEntity entity) => Set.Contains(entity);
        /// <summary>
        /// 是否有一项满足条件(异步)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = default) => predicate == default ? Set.AnyAsync(TokenSource.Token) : Set.AnyAsync(predicate, TokenSource.Token);
        /// <summary>
        /// 是否有一项满足条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual bool Any(Expression<Func<TEntity, bool>>? predicate = default) => predicate == default ? Set.Any() : Set.Any(predicate);
        /// <summary>
        /// 计数(异步)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = default) => predicate == default ? Set.CountAsync(TokenSource.Token) : Set.CountAsync(predicate, TokenSource.Token);
        /// <summary>
        /// 计数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<TEntity, bool>>? predicate = default) => predicate == default ? Set.Count() : Set.Count(predicate);
        /// <summary>
        /// 创建(异步)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> CreateAsync(TEntity entity)
        {
            await Set.AddAsync(entity, TokenSource.Token);
            return await Work.AutoSaveChangesAsync();
        }
        /// <summary>
        /// 创建(异步)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<int> CreateAsync(IEnumerable<TEntity> entities)
        {
            await Set.AddRangeAsync(entities, TokenSource.Token);
            return await Work.AutoSaveChangesAsync();
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Create(TEntity entity)
        {
            Set.Add(entity);
            return Work.AutoSaveChanges();
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int Create(IEnumerable<TEntity> entities)
        {
            Set.AddRange(entities);
            return Work.AutoSaveChanges();
        }
        /// <summary>
        /// 删除(异步)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public virtual Task<int> DeleteAsync(TEntity entity)
        {
            Set.Remove(entity);
            return Work.AutoSaveChangesAsync();
        }
        /// <summary>
        /// 删除(异步)
        /// </summary>
        /// <param name="entities"></param>
        public virtual Task<int> DeleteAsync(IEnumerable<TEntity> entities)
        {
            Set.RemoveRange(entities);
            return Work.AutoSaveChangesAsync();
        }
        /// <summary>
        /// 根据主键删除(异步)
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(TKey[] arrayKeyValue)
        {
            foreach (var item in arrayKeyValue)
            {
                var obj = await FindAsync(item);
                if (obj != default)
                    Work.Context.Remove(obj);
            }
            return await Work.AutoSaveChangesAsync();
        }
        /// <summary>
        /// 根据主键删除(异步)
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(TKey keyValue) => DeleteAsync(new[] { keyValue });
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public virtual int Delete(TEntity entity)
        {
            Set.Remove(entity);
            return Work.AutoSaveChanges();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities"></param>
        public virtual int Delete(IEnumerable<TEntity> entities)
        {
            Set.RemoveRange(entities);
            return Work.AutoSaveChanges();
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <returns></returns>
        public virtual int Delete(TKey[] arrayKeyValue)
        {
            foreach (var item in arrayKeyValue)
            {
                var obj = Find(item);
                if (obj != default)
                    Work.Context.Remove(obj);
            }
            return Work.AutoSaveChanges();
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public virtual int Delete(TKey keyValue) => Delete(new[] { keyValue });
        /// <summary>
        /// 修改(异步)
        /// </summary>
        /// <param name="entities"></param>
        public virtual async Task<int> UpdateAsync(IEnumerable<TEntity> entities)
        {
            Set.UpdateRange(entities);
            return await Work.AutoSaveChangesAsync();
        }
        /// <summary>
        /// 修改(异步)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            Set.Update(entity);
            return await Work.AutoSaveChangesAsync();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entities"></param>
        public virtual int Update(IEnumerable<TEntity> entities)
        {
            Set.UpdateRange(entities);
            return Work.AutoSaveChanges();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Update(TEntity entity)
        {
            Set.Update(entity);
            return Work.AutoSaveChanges();
        }
        /// <summary>
        /// 根据主键查找(异步)
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public virtual async Task<TEntity?> FindAsync(TKey keyValue)
        {
            if (keyValue is ITuple tup)
            {
                var array = new object?[tup.Length];
                for (int i = 0; i < tup.Length; i++)
                {
                    array[i] = tup[i];
                }
                return await Set.FindAsync(array, TokenSource.Token);
            }
            else
            {
                return await Set.FindAsync(new object[] { keyValue }, TokenSource.Token);
            }
        }
        /// <summary>
        /// 根据主键查找(异步)
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>?> FindAsync(TKey[] arrayKeyValue)
        {
            var whereExp = ExpressionHelper.WhereEqualOr<TEntity, TKey>(Keys, arrayKeyValue);
            return await Set.Where(whereExp).ToArrayAsync(TokenSource.Token);
        }
        /// <summary>
        /// 根据主键查找
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public virtual TEntity? Find(TKey keyValue)
        {
            if (keyValue is ITuple tup)
            {
                var array = new object?[tup.Length];
                for (int i = 0; i < tup.Length; i++)
                {
                    array[i] = tup[i];
                }
                return Set.Find(array);
            }
            else
            {
                return Set.Find(keyValue);
            }
        }
        /// <summary>
        /// 根据主键查找
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity>? Find(TKey[] arrayKeyValue)
        {
            var whereExp = ExpressionHelper.WhereEqualOr<TEntity, TKey>(Keys, arrayKeyValue);
            return Set.Where(whereExp).ToArray();
        }
        /// <summary>
        /// 析构
        /// </summary>
        ~RepositoryBase() => Dispose();
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
