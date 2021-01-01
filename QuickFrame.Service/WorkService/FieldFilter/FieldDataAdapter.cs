using DynamicTypeMap;
using QuickFrame.Common;
using QuickFrame.Repository;
using System;
using System.Linq;

namespace QuickFrame.Service
{
    /// <summary>
    /// 列数据适配器
    /// </summary>
    [ScopeInjection]
    public class FieldDataAdapter : IDataAdapter
    {
        private readonly IFieldFilterRepository _filterRepository;

        public FieldDataAdapter(IFieldFilterRepository filterRepository)
        {
            _filterRepository = filterRepository;
        }
        /// <summary>
        /// 获取有效列
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="newTypeName"></param>
        /// <returns></returns>
        public string[]? GetNewPropertys(Type sourceType, string newTypeName)
        {
            var sourceName = sourceType.FullName;
            if (sourceName != default)
            {
                var field = _filterRepository.GetFields(sourceName, newTypeName);
                if (field != default)
                {
                    return field;
                }
            }
            return sourceType.GetFields().Select(x => x.Name).ToArray();
        }
    }
}
