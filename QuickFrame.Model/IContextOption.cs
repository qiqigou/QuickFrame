namespace QuickFrame.Model
{
    /// <summary>
    /// 后台库标志
    /// </summary>
    public class BackOption : IContextOption { }
    /// <summary>
    /// 业务库标志
    /// </summary>
    public class WorkOption : IContextOption { }
    /// <summary>
    /// DbContext标志(用于区分不同的DbContext)
    /// </summary>
    public interface IContextOption { }
}
