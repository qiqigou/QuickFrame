using QuickFrame.Common;

namespace QuickFrame.Model
{
    /// <summary>
    /// 数据库实体标识(含库标志)
    /// </summary>
    /// <typeparam name="TOption">库标志</typeparam>
    public interface IDbEntity<TOption> : IDbEntity where TOption : IContextOption { }
    /// <summary>
    /// 数据库实体标识
    /// </summary>
    public interface IDbEntity : IMEntity { }
}
