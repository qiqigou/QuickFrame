using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using QuickFrame.Common;
using QuickFrame.Model;
using QuickFrame.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickFrame.Service
{
    /// <summary>
    /// 单表服务抽象类
    /// </summary>
    /// <typeparam name="TEntity">表</typeparam>
    /// <typeparam name="TInput">新增时输入模型</typeparam>
    /// <typeparam name="TUpdInput">修改时输入模型</typeparam>
    /// <typeparam name="TView">视图</typeparam>
    /// <typeparam name="TKey">主键</typeparam>
    public abstract class BillServiceBase<TEntity, TInput, TUpdInput, TView, TKey> : IHandle<TInput, TUpdInput, TKey>, IQuery<TView, TKey>
        where TEntity : WithStampTable, new()
        where TInput : IDataInput, new()
        where TUpdInput : WithStampDataInput, new()
        where TView : WithStampView, new()
        where TKey : notnull
    {
        private static Func<TEntity, TKey>? _keyFunc;
        /// <summary>
        /// 服务提供者
        /// </summary>
        protected readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 映射器
        /// </summary>
        protected readonly IMapper _mapper;
        /// <summary>
        /// 仓储
        /// </summary>
        protected readonly IRepositoryBase<TEntity, TKey> _repository;
        /// <summary>
        /// 视图查询提供工厂
        /// </summary>
        protected readonly IQueryFactory _queryFactory;
        /// <summary>
        /// 视图查询提供者
        /// </summary>
        protected abstract IQueryProvider<TView, TKey> Query(IQueryFactory queryFactory);
        /// <summary>
        /// 构建单表服务
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        public BillServiceBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _queryFactory = serviceProvider.GetRequiredService<IQueryFactory>();
            _repository = serviceProvider.GetRequiredService<IRepositoryBase<TEntity, TKey>>();
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TKey> CreateAsync(TInput input)
        {
            var entity = _mapper.Map<TEntity>(input);
            await _repository.CreateAsync(entity);
            return KeyFunc.Invoke(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(TKey keyValue, TUpdInput input)
        {
            var res = await _repository.FindAsync(keyValue);
            _ = res ?? throw new HandelException(MessageCodeOption.Bad_Delete, keyValue);
            _ = input.Timestamp.ToBase64() == res.timestamp.ToBase64() ? true : throw new HandelException(MessageCodeOption.Bad_Update, keyValue);
            _mapper.Map(input, res);
            return await _repository.UpdateAsync(res);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(TKey[] arrayKeyValue)
        {
            var count = await _repository.DeleteAsync(arrayKeyValue);
            return count > 0 ? count : throw new HandelException(MessageCodeOption.Bad_Delete, arrayKeyValue);
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public virtual Task<TView?> SingleAsync(TKey keyValue) => Query(_queryFactory).SingleAsync(keyValue);
        /// <summary>
        /// 查询视图集合
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public virtual Task<IEnumerable<TView>> QueryAsync(TKey keyValue) => Query(_queryFactory).QueryAsync(keyValue);
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual Task<PageOutput<TView>> ObjQueryAsync(ObjFilterInput input) => Query(_queryFactory).QueryAsync(input);
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual Task<PageOutput<TView>> SQLQueryAsync(SQLFilterInput input) => Query(_queryFactory).QueryAsync(input);
        /// <summary>
        /// 主键取值的委托
        /// </summary>
        protected Func<TEntity, TKey> KeyFunc => _keyFunc ??= ExpressionHelper.MemberLambda<TEntity, TKey>(_repository.Keys).Compile();
    }
}
