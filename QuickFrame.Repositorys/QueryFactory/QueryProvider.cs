using Microsoft.EntityFrameworkCore;
using QuickFrame.Common;
using QuickFrame.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 提供通用的视图查询
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TQKey"></typeparam>
    public class QueryProvider<TView, TQKey> : IQueryProvider<TView, TQKey>
        where TView : ViewEntity, new()
        where TQKey : notnull
    {
        private readonly IQueryRepository _repository;
        private readonly string[] _qkeyMap;
        private readonly SortInput[] _orderMap;
        /// <summary>
        /// 构建提供通用的视图查询器
        /// </summary>
        /// <param name="repository">视图查询仓储</param>
        /// <param name="qkeyMap">查询键</param>
        /// <param name="orderMap">排序键</param>
        public QueryProvider(IQueryRepository repository, string[] qkeyMap, string[] orderMap)
        {
            _repository = repository;
            _qkeyMap = qkeyMap;
            _orderMap = orderMap.Select(item => new SortInput { OrderBy = item }).ToArray();
        }
        /// <summary>
        /// 构建提供通用的视图查询器
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="qkeyMap"></param>
        /// <param name="orderMap"></param>
        public QueryProvider(IQueryRepository repository, string[] qkeyMap, SortInput[] orderMap)
        {
            _repository = repository;
            _qkeyMap = qkeyMap;
            _orderMap = orderMap;
        }
        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="qkeyValue"></param>
        /// <returns></returns>
        public async Task<TView?> SingleAsync(TQKey qkeyValue)
        {
            var whereExpr = ExpressionHelper.WhereLambda<TView, TQKey>(_qkeyMap, qkeyValue);
            return await _repository.Select<TView>().SingleOrDefaultAsync(whereExpr);
        }
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="qkeyValue"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TView>> QueryAsync(TQKey qkeyValue)
        {
            var whereExpr = ExpressionHelper.WhereLambda<TView, TQKey>(_qkeyMap, qkeyValue);
            return await ObjFilterConvertHelper.OrderBySortInput(_repository.Select<TView>(), _orderMap).Where(whereExpr).ToArrayAsync();
        }
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TView>> QueryAsync(TQKey[] arrayKeyValue)
        {
            var whereExpr = ExpressionHelper.WhereEqualOr<TView, TQKey>(_qkeyMap, arrayKeyValue);
            return await ObjFilterConvertHelper.OrderBySortInput(_repository.Select<TView>(), _orderMap).Where(whereExpr).ToArrayAsync();
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PageOutput<TView>> QueryAsync(ObjFilterInput input)
        {
            var query = _repository.Select<TView>().QueryByGroupInput(input.Condition);
            var total = await query.CountAsync();
            var page = PageInput.Convert(total, input.Page, _orderMap);
            var data = await query.QueryByPageInput(page).ToArrayAsync();
            return PageOutput.Convert(total, page, data);
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PageOutput<TView>> QueryAsync(SQLFilterInput input)
        {
            var query = _repository.Select<TView>().QueryBySQLWhere(input.Condition);
            var total = await query.CountAsync();
            var page = PageInput.Convert(total, input.Page, _orderMap);
            var data = await query.QueryByPageInput(page).ToArrayAsync();
            return PageOutput.Convert(total, page, data);
        }
    }
}
