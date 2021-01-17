using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using QuickFrame.Models;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 表仓储接口
    /// </summary>
    /// <typeparam name="TEntity">表类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public interface IRepositoryBase<TEntity, TKey> : IRepository
        where TEntity : TableEntity, new()
        where TKey : notnull
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        IUnitOfWork Work { get; }
        /// <summary>
        /// 主键
        /// </summary>
        string[] Keys { get; }
        /// <summary>
        /// 根据主键删除实体并保存后事件
        /// </summary>
        event EventHandler<IEnumerable<TEntity>>? DeletedByKey;
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Select { get; }
        /// <summary>
        /// 创建(异步)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> CreateAsync(TEntity entity);
        /// <summary>
        /// 创建(异步)
        /// </summary>
        /// <returns></returns>
        Task<int> CreateAsync(IEnumerable<TEntity> entities);
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Create(TEntity entity);
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        int Create(IEnumerable<TEntity> entities);
        /// <summary>
        /// 删除(异步)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(TEntity entity);
        /// <summary>
        /// 删除(异步)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(IEnumerable<TEntity> entities);
        /// <summary>
        /// 根据主键删除(异步)
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <param name="deletedCallback"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(TKey[] arrayKeyValue, Action<IEnumerable<TEntity>>? deletedCallback = default);
        /// <summary>
        /// 根据主键删除(异步)
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="deletedCallback"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(TKey keyValue, Action<TEntity>? deletedCallback = default);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Delete(TEntity entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        int Delete(IEnumerable<TEntity> entities);
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <param name="deletedCallback"></param>
        /// <returns></returns>
        int Delete(TKey[] arrayKeyValue, Action<IEnumerable<TEntity>>? deletedCallback = default);
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="deletedCallback"></param>
        /// <returns></returns>
        int Delete(TKey keyValue, Action<TEntity>? deletedCallback = default);
        /// <summary>
        /// 修改(异步)
        /// </summary>
        /// <param name="entity">待修改实体</param>
        Task<int> UpdateAsync(TEntity entity);
        /// <summary>
        /// 修改(异步)
        /// </summary>
        /// <param name="entities">待修改实体集合</param>
        Task<int> UpdateAsync(IEnumerable<TEntity> entities);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">待修改实体</param>
        int Update(TEntity entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entities">待修改实体集合</param>
        int Update(IEnumerable<TEntity> entities);
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">基本类型或者SqlParameter参数</param>
        /// <returns></returns>
        int ExecuteSqlRaw(string sql, params object[] parameters);
        /// <summary>
        /// 执行SQL语句(异步)
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">基本类型或者SqlParameter参数</param>
        /// <returns></returns>
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
        /// <summary>
        /// 根据主键查找(异步)
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        Task<TEntity?> FindAsync(TKey keyValue);
        /// <summary>
        /// 根据主键查找(异步)
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>?> FindAsync(TKey[] arrayKeyValue);
        /// <summary>
        /// 根据主键查找
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        TEntity? Find(TKey keyValues);
        /// <summary>
        /// 根据主键查找
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <returns></returns>
        IEnumerable<TEntity>? Find(TKey[] arrayKeyValue);
        /// <summary>
        /// 包含(异步)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> ContainsAsync(TEntity entity);
        /// <summary>
        /// 包含
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Contains(TEntity entity);
        /// <summary>
        /// 是否有一项满足条件(异步)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = default);
        /// <summary>
        /// 是否有一项满足条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Any(Expression<Func<TEntity, bool>>? predicate = default);
        /// <summary>
        /// 计数(异步)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = default);
        /// <summary>
        /// 计数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>>? predicate = default);
    }
}
