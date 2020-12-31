using IdentityModel;

namespace QuickFrame.Common
{
    /// <summary>
    /// 用户信息常量
    /// </summary>
    public static class UserOptions
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public const string UserId = JwtClaimTypes.Subject;
        /// <summary>
        /// 用户名
        /// </summary>
        public const string UserName = JwtClaimTypes.Name;
        /// <summary>
        /// 角色
        /// </summary>
        public const string Role = JwtClaimTypes.Role;
        /// <summary>
        /// 数据库名
        /// </summary>
        public const string DBName = "db";
        /// <summary>
        /// 数据签名
        /// </summary>
        public const string Sign = "sign";
        /// <summary>
        /// 刷新Token过期时间
        /// </summary>
        public const string RefreshToken = "refexp";
    }
}
