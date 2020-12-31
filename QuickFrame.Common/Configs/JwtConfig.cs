namespace QuickFrame.Common
{
    /// <summary>
    /// Jwt配置
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; } = string.Empty;
        /// <summary>
        /// 订阅者
        /// </summary>
        public string Audience { get; set; } = string.Empty;
        /// <summary>
        /// 密钥
        /// </summary>
        public string SecurityKey { get; set; } = string.Empty;
        /// <summary>
        /// 有效期(分钟)
        /// </summary>
        public int Expires { get; set; }
        /// <summary>
        /// 刷新有效期(分钟)
        /// </summary>
        public int RefreshExpires { get; set; }
    }
}
