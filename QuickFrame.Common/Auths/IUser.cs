namespace QuickFrame.Common
{
    /// <summary>
    /// 用户信息接口
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        string Id { get; }
        /// <summary>
        /// 用户名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 角色
        /// </summary>
        /// <value></value>
        string Role { get; }
        /// <summary>
        /// 数据库名
        /// </summary>
        string DBName { get; }
        /// <summary>
        /// 数据签名
        /// </summary>
        string Sign { get; }
    }
}
