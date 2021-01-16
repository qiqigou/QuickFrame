using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using QuickFrame.Common;
using QuickFrame.IServices;
using QuickFrame.Models;
using QuickFrame.Repositorys;

namespace QuickFrame.Services
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
        where TUpdInput : IDataInput, new()
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
        /// <param name="timestamp"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(TKey keyValue, byte[] timestamp, TUpdInput input)
        {
            var res = await _repository.FindAsync(keyValue);
            _ = res ?? throw new HandelException(MessageCodeOption.Bad_Delete, keyValue);
            _ = res.timestamp.ToBase64() == timestamp.ToBase64() ? true : throw new HandelException(MessageCodeOption.Bad_Update, keyValue);
            _mapper.Map(input, res);
            return await _repository.UpdateAsync(res);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="arrayKeyStamp"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(KeyStamp<TKey>[] arrayKeyStamp)
        {
            var keys = arrayKeyStamp.Select(x => x.Key).ToArray();
            var array = await _repository.FindAsync(keys);
            _ = array ?? throw new HandelArrayException(MessageCodeOption.Bad_Delete, keys);
            var bad_keys = array
                .Join(arrayKeyStamp, KeyFunc, y => y.Key, (x, y) => new { y.Key, y.Timpstamp, x.timestamp })
                .Where(x => x.Timpstamp.ToBase64() != x.timestamp.ToBase64())
                .Select(x => x.Key)
                .ToArray();
            _ = bad_keys.Any() ? throw new HandelArrayException(MessageCodeOption.Bad_Update, bad_keys) : true;
            var count = await _repository.DeleteAsync(keys);
            return count > 0 ? count : throw new HandelArrayException(MessageCodeOption.Bad_Delete, keys);
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
