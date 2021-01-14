using Microsoft.EntityFrameworkCore;
using QuickFrame.Common;
using QuickFrame.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 表仓储抽象
    /// </summary>
    internal abstract class RepositoryBase<TEntity, TKey> : Repository, IRepositoryBase<TEntity, TKey>
        where TEntity : TableEntity, new()
        where TKey : notnull
    {
        private static string[]? _keys;
        private static Func<TEntity, TKey>? _keyFunc;
        protected DbSet<TEntity> Set;
        public event EventHandler<IEnumerable<TEntity>>? DeletedByKey;

        protected RepositoryBase(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            Set = unitOfWork.Context.Set<TEntity>();
        }
        /// <summary>
        /// 主表主键取值委托
        /// </summary>
        protected Func<TEntity, TKey> KeyFunc => _keyFunc ??= ExpressionHelper.MemberLambda<TEntity, TKey>(Keys).Compile();
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
        public string[] Keys => _keys ??= Work.Context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties.Select(x => x.Name).ToArray();
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
        /// <param name="entities"></param>
        public virtual async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
        {
            Set.RemoveRange(entities);
            int count;
            try
            {
                count = await Work.AutoSaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var keys = GetKeysByDbUpdateConcurrencyException(ex);
                throw new HandelArrayException(MessageCodeOption.Bad_Update, keys);
            }
            return count;
        }
        /// <summary>
        /// 删除(异步)
        /// </summary>
        /// <param name="entity"></param>
        public virtual Task<int> DeleteAsync(TEntity entity) => DeleteAsync(new[] { entity });
        /// <summary>
        /// 根据主键删除(异步)
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <param name="deletedCallback"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(TKey[] arrayKeyValue, Action<IEnumerable<TEntity>>? deletedCallback = default)
        {
            var array = await FindAsync(arrayKeyValue);
            Set.RemoveRange(array);
            int count;
            try
            {
                count = await Work.AutoSaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var keys = GetKeysByDbUpdateConcurrencyException(ex);
                throw new HandelArrayException(MessageCodeOption.Bad_Update, keys);
            }
            if ((array?.Any() ?? false) && count > 0)
            {
                deletedCallback?.Invoke(array);
                DeletedByKey?.Invoke(this, array);
            }
            return count;
        }
        /// <summary>
        /// 根据主键删除(异步)
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="deletedCallback"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(TKey keyValue, Action<TEntity>? deletedCallback = default)
        {
            if (deletedCallback != default)
            {
                return DeleteAsync(new[] { keyValue }, array => array.ForEach(deletedCallback));
            }
            return DeleteAsync(new[] { keyValue });
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities"></param>
        public virtual int Delete(IEnumerable<TEntity> entities)
        {
            Set.RemoveRange(entities);
            int count;
            try
            {
                count = Work.AutoSaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var keys = GetKeysByDbUpdateConcurrencyException(ex);
                throw new HandelArrayException(MessageCodeOption.Bad_Update, keys);
            }
            return count;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public virtual int Delete(TEntity entity) => Delete(new[] { entity });
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <param name="deletedCallback">删除后的回调</param>
        /// <returns></returns>
        public virtual int Delete(TKey[] arrayKeyValue, Action<IEnumerable<TEntity>>? deletedCallback = default)
        {
            var array = Find(arrayKeyValue);
            Set.RemoveRange(array);
            int count;
            try
            {
                count = Work.AutoSaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var keys = GetKeysByDbUpdateConcurrencyException(ex);
                throw new HandelArrayException(MessageCodeOption.Bad_Update, keys);
            }
            if ((array?.Any() ?? false) && count > 0)
            {
                deletedCallback?.Invoke(array);
                DeletedByKey?.Invoke(this, array);
            }
            return count;
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="deletedCallback"></param>
        /// <returns></returns>
        public virtual int Delete(TKey keyValue, Action<TEntity>? deletedCallback = default)
        {
            if (deletedCallback != default)
            {
                return Delete(new[] { keyValue }, array => array.ForEach(deletedCallback));
            }
            return Delete(new[] { keyValue });
        }
        /// <summary>
        /// 修改(异步)
        /// </summary>
        /// <param name="entities"></param>
        public virtual async Task<int> UpdateAsync(IEnumerable<TEntity> entities)
        {
            Set.UpdateRange(entities);
            int count;
            try
            {
                count = await Work.AutoSaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var keys = GetKeysByDbUpdateConcurrencyException(ex);
                throw new HandelArrayException(MessageCodeOption.Bad_Update, keys);
                throw;
            }
            return count;
        }
        /// <summary>
        /// 修改(异步)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateAsync(TEntity entity) => UpdateAsync(new[] { entity });
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entities"></param>
        public virtual int Update(IEnumerable<TEntity> entities)
        {
            Set.UpdateRange(entities);
            int count;
            try
            {
                count = Work.AutoSaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var keys = GetKeysByDbUpdateConcurrencyException(ex);
                throw new HandelArrayException(MessageCodeOption.Bad_Update, keys);
            }
            return count;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Update(TEntity entity) => Update(new[] { entity });
        /// <summary>
        /// 根据主键查找(异步)
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public virtual async Task<TEntity?> FindAsync(TKey keyValue)
        {
            if (keyValue is ITuple tuple)
            {
                return await Set.FindAsync(tuple.ToArray(), TokenSource.Token);
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
            if (keyValue is ITuple tuple)
            {
                return Set.Find(tuple.ToArray());
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
        /// 获取并发乐观锁异常的数据Key集合
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private ArrayList GetKeysByDbUpdateConcurrencyException(DbUpdateConcurrencyException ex)
        {
            var keyarray = new ArrayList(ex.Entries.Count);
            foreach (var item in ex.Entries)
            {
                if (item.Entity is TEntity entity)
                {
                    var key = KeyFunc.Invoke(entity);
                    keyarray.Add(key);
                }
            }
            return keyarray;
        }
        /// <summary>
        /// 析构
        /// </summary>
        ~RepositoryBase() => Dispose();
    }
}
