using QuickFrame.Common;

namespace System.Linq
{
    /// <summary>
    /// linq扩展
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// 条件查询(对象形式)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static IQueryable<TSource> QueryByGroupInput<TSource>(this IQueryable<TSource> source, GroupInput? group)
            where TSource : class, new()
        {
            var lambda = ObjFilterConvertHelper.ConvertToExpression<TSource>(group);
            return source.Where(lambda);
        }
        /// <summary>
        /// 条件查询(SQL形式)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sqlwhere"></param>
        /// <returns></returns>
        public static IQueryable<TSource> QueryBySQLWhere<TSource>(this IQueryable<TSource> source, string sqlwhere)
            where TSource : class, new()
        {
            var lambda = SQLFilterConvertHelper.ConvertToExpression<TSource>(sqlwhere);
            return source.Where(lambda);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static IQueryable<TSource> QueryByPageInput<TSource>(this IQueryable<TSource> source, PageInput page)
            where TSource : class, new()
        {
            return ObjFilterConvertHelper.ConvertSortInput(source, page.Sort).Page(page.Index, page.Size);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int index, int size)
            where TSource : class
        {
            return source.Skip((index - 1) * size).Take(size);
        }
    }
}
