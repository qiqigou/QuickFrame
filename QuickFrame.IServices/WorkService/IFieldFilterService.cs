using QuickFrame.Models;

namespace QuickFrame.IServices
{
    /// <summary>
    /// 字段过滤器
    /// </summary>
    public interface IFieldFilterService : IService, IHandleMainChild<FieldFilterInput, FieldFilterUpdInput, FieldFiltercInput, FieldFiltercUpdInput, long>, IQueryMainChild<v_fieldfilter, v_fieldfilterc, long>
    {

    }
}
