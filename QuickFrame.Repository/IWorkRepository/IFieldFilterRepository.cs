using QuickFrame.Model;

namespace QuickFrame.Repository
{
    /// <summary>
    /// 字段过滤仓储
    /// </summary>
    public interface IFieldFilterRepository : IRepositoryBase<fieldfilter_fg, long>
    {
        string[]? GetFields(string sourceName, string destName);
    }
    /// <summary>
    /// 字段过滤子仓储
    /// </summary>
    public interface IFieldFilterDtlRepository : IRepositoryBase<fieldfilterc_fgc, (long, int)>
    {

    }
}
