using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuickFrame.Common;
using QuickFrame.IServices;
using QuickFrame.Models;
using QuickFrame.Repositorys;

namespace QuickFrame.Services
{
    /// <summary>
    /// 主子表服务抽象类
    /// </summary>
    /// <typeparam name="TMain">主表</typeparam>
    /// <typeparam name="TChild">子表</typeparam>
    /// <typeparam name="TMainView">主视图</typeparam>
    /// <typeparam name="TChildView">子视图</typeparam>
    /// <typeparam name="TMainInput">新增主表时输入模型</typeparam>
    /// <typeparam name="TMainUpdInput">修改主表时输入模型</typeparam>
    /// <typeparam name="TChildInput">新增子表时输入模型</typeparam>
    /// <typeparam name="TChildUpdInput">修改子表时输入模型</typeparam>
    /// <typeparam name="TKey">主表主键</typeparam>
    /// <typeparam name="TCKey">子表项次键</typeparam>
    public abstract class BillMainChildServiceBase<TMain, TChild, TMainView, TChildView, TMainInput, TMainUpdInput, TChildInput, TChildUpdInput, TKey, TCKey> : IHandleMainChild<TMainInput, TMainUpdInput, TChildInput, TChildUpdInput, TKey>, IQueryMainChild<TMainView, TChildView, TKey>
        where TMain : WithStampTable, new()
        where TChild : TableEntity, new()
        where TMainInput : IDataInput, new()
        where TMainUpdInput : IDataInput, new()
        where TChildInput : IDataInput, new()
        where TChildUpdInput : IDataInput, new()
        where TMainView : WithStampView, new()
        where TChildView : ViewEntity, new()
        where TKey : notnull
        where TCKey : notnull
    {
        private static Func<TMain, TKey>? _mainKeyFunc;
        private static Func<TMain, IEnumerable<TChild>?>? _childTableFunc;
        private static Action<TMain, IEnumerable<TChild>?>? _childTableAction;
        /// <summary>
        /// 服务提供者
        /// </summary>
        protected readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 映射器
        /// </summary>
        protected readonly IMapper _mapper;
        /// <summary>
        /// 视图查询提供工厂
        /// </summary>
        protected readonly IQueryFactory _queryFactory;
        /// <summary>
        /// 主表仓储
        /// </summary>
        protected readonly IRepositoryBase<TMain, TKey> _mainRepository;
        /// <summary>
        /// 构建主子表服务
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        public BillMainChildServiceBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _queryFactory = serviceProvider.GetRequiredService<IQueryFactory>();
            _mainRepository = serviceProvider.GetRequiredService<IRepositoryBase<TMain, TKey>>();
        }
        /// <summary>
        /// 主视图查询提供者
        /// </summary>
        protected abstract IQueryProvider<TMainView, TKey> QueryMain(IQueryFactory queryFactory);
        /// <summary>
        /// 子视图查询提供者
        /// </summary>
        protected abstract IQueryProvider<TChildView, TKey> QueryChild(IQueryFactory queryFactory);
        /// <summary>
        /// 主表中的子表字段
        /// </summary>
        protected abstract Expression<Func<TMain, IEnumerable<TChild>?>> ChildTable { get; }
        /// <summary>
        /// 子表项次
        /// </summary>
        protected abstract Func<TChild, TCKey> ChildIorder { get; }
        /// <summary>
        /// 子表修改时输入模型项次
        /// </summary>
        protected abstract Func<TChildUpdInput, TCKey> ChildUpdInputIorder { get; }
        /// <summary>
        /// 查询子视图集合
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public virtual Task<IEnumerable<TChildView>> QueryChildAsync(TKey keyValue)
            => QueryChild(_queryFactory).QueryAsync(keyValue);
        /// <summary>
        /// 主视图条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual Task<PageOutput<TMainView>> ObjQueryMainAsync(ObjFilterInput input)
            => QueryMain(_queryFactory).QueryAsync(input);
        /// <summary>
        /// 主视图条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual Task<PageOutput<TMainView>> SQLQueryMainAsync(SQLFilterInput input)
            => QueryMain(_queryFactory).QueryAsync(input);
        /// <summary>
        /// 获取主视图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public virtual Task<TMainView?> SingleMainAsync(TKey keyValue)
            => QueryMain(_queryFactory).SingleAsync(keyValue);
        /// <summary>
        /// 获取主子视图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public virtual async Task<MainChildOutput<TMainView, TChildView>?> SingleMainChildAsync(TKey keyValue)
        {
            var main = await QueryMain(_queryFactory).SingleAsync(keyValue);
            if (main == default) return default;
            var child = await QueryChild(_queryFactory).QueryAsync(keyValue);
            return new MainChildOutput<TMainView, TChildView> { Main = main, Child = child };
        }
        /// <summary>
        /// 删除主子表
        /// </summary>
        /// <remarks>
        /// 如果使用EFCore创建数据库，则会配置级联删除
        /// </remarks>
        /// <param name="arrayKeyStamp"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteMainChildAsync(KeyStamp<TKey>[] arrayKeyStamp)
        {
            var keys = arrayKeyStamp.Select(x => x.Key).ToArray();
            var array = await _mainRepository.FindAsync(keys);
            _ = array ?? throw new HandelArrayException(MessageCodeOption.Bad_Delete, keys);
            var bad_keys = array
                .Join(arrayKeyStamp, MainKeyFunc, y => y.Key, (x, y) => new { y.Key, y.Timpstamp, x.timestamp })
                .Where(x => x.Timpstamp.ToBase64() != x.timestamp.ToBase64())
                .Select(x => x.Key)
                .ToArray();
            _ = bad_keys.Any() ? throw new HandelArrayException(MessageCodeOption.Bad_Update, bad_keys) : true;
            var count = await _mainRepository.DeleteAsync(keys);
            return count > 0 ? count : throw new HandelArrayException(MessageCodeOption.Bad_Delete, keys);
        }
        /// <summary>
        /// 创建主子表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TKey> CreateMainChildAsync(MainChildInput<TMainInput, TChildInput> input)
        {
            var main = _mapper.Map<TMain>(input.Main);
            var set = input.Child.Select(x => _mapper.Map<TChild>(x)).ToList();
            ChildTableAction(main, set);
            await _mainRepository.CreateAsync(main);
            return MainKeyFunc.Invoke(main);
        }
        /// <summary>
        /// 修改主子表
        /// </summary>
        /// <remarks>
        /// 1.使用对比的方式实现的修改
        /// 2.前端只需要根据输入模型传入数据即可.这样相对安全
        /// 3.如果传入的某个字段值为空,则不会修改该字段的值(可以在对应的Map映射中禁用该策略)
        /// </remarks>
        /// <param name="keyValue"></param>
        /// <param name="timestamp"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateMainChildAsync(TKey keyValue, byte[] timestamp, MainChildInput<TMainUpdInput, TChildUpdInput> input)
        {
            var whereExpr = ExpressionHelper.WhereLambda<TMain, TKey>(_mainRepository.Keys, keyValue);
            var res = await _mainRepository.Select.Include(ChildTable).SingleOrDefaultAsync(whereExpr);
            _ = res ?? throw new HandelException(MessageCodeOption.Bad_Delete, keyValue);
            _ = res.timestamp.ToBase64() == timestamp.ToBase64() ? true : throw new HandelException(MessageCodeOption.Bad_Update, keyValue);
            _mapper.Map(input.Main, res);
            var child = ChildTableFunc.Invoke(res)?.ToList();
            if (input.Child.Any())
            {
                if (child?.Any() ?? false)
                {
                    var arrayput = input.Child.Select(ChildUpdInputIorder).ToArray();
                    var arraydb = child.Select(ChildIorder).ToArray();
                    //修改项
                    var upd = input.Child.Join(child, ChildUpdInputIorder, ChildIorder, (put, db) => new { put, db }).ToArray();
                    upd.ForEach(x => _mapper.Map(x.put, x.db));
                    //新增项
                    var add = input.Child.Where(x => !arraydb.Contains(ChildUpdInputIorder.Invoke(x))).ToArray();
                    add.ForEach(x => child.Add(_mapper.Map<TChild>(x)));
                    //删除项
                    var del = child.Where(x => !arrayput.Contains(ChildIorder.Invoke(x))).ToArray();
                    del.ForEach(x => child.Remove(x));
                    ChildTableAction.Invoke(res, child);
                }
                else
                {
                    child = input.Child.Select(x => _mapper.Map<TChild>(x)).ToList();
                    ChildTableAction.Invoke(res, child);
                }
            }
            else
            {
                ChildTableAction.Invoke(res, default);
            }
            return await _mainRepository.UpdateAsync(res);
        }
        /// <summary>
        /// 主表主键取值委托
        /// </summary>
        protected Func<TMain, TKey> MainKeyFunc => _mainKeyFunc ??= ExpressionHelper.MemberLambda<TMain, TKey>(_mainRepository.Keys).Compile();
        /// <summary>
        /// 主表中取子表值委托
        /// </summary>
        protected Func<TMain, IEnumerable<TChild>?> ChildTableFunc => _childTableFunc ??= ChildTable.Compile();
        /// <summary>
        /// 子表赋值的委托
        /// </summary>
        protected Action<TMain, IEnumerable<TChild>?> ChildTableAction => _childTableAction ??= ExpressionHelper.AssignLambda<TMain, IEnumerable<TChild>>(((MemberExpression)ChildTable.Body).Member.Name).Compile();
    }
}
