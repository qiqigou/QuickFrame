namespace QuickFrame.Common
{
    /// <summary>
    /// 数据缓存键(可以在内存和Redis中切换)
    /// </summary>
    public static class CacheKey
    {
        /// <summary>
        /// 验证码 system:verify:code:{guid}
        /// </summary>
        public const string VerifyCodeKey = "system:verify:code:{0}";
        /// <summary>
        /// 密码加密 system:password:encrypt:{guid}
        /// </summary>
        public const string PassWordEncryptKey = "system:password:encrypt:{0}";
        /// <summary>
        /// 用户权限 system:user:{用户主键}:permissions
        /// </summary>
        public const string UserPermissions = "system:user:{0}:permissions";
        /// <summary>
        /// 实体注释 system:memberitems
        /// </summary>
        public const string ModelMemberItems = "system:memberitems";
        /// <summary>
        /// 实体列 system:columnitems:{实体名称}
        /// </summary>
        public const string ColumnItems = "system:columnitems:{0}";
        /// <summary>
        /// 实体名
        /// </summary>
        public const string ViewNames = "system:viewnames";
    }
}
