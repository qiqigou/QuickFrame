using System.Collections.Generic;

namespace QuickFrame.Common
{
    /// <summary>
    /// 实体过滤提供者
    /// </summary>
    public interface IFieldFilterProvider
    {
        /// <summary>
        /// 实体过滤
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="data"></param>
        /// <param name="newTypeName"></param>
        /// <returns></returns>
        object? Filter<TModel>(TModel? data, string? newTypeName = default) where TModel : class;
        /// <summary>
        /// 实体过滤(集合)
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="data"></param>
        /// <param name="newTypeName"></param>
        /// <returns></returns>
        IEnumerable<object>? FilterRange<TModel>(IEnumerable<TModel?>? data, string? newTypeName = default) where TModel : class;
        /// <summary>
        /// 移除缓存中的新类型
        /// </summary>
        /// <param name="sourceTypeName">源类型名称</param>
        /// <param name="newTypeName">新类型名称</param>
        bool RemoveNewType(string sourceTypeName, string? newTypeName = default);
    }
}
