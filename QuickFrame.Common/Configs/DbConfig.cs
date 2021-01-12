namespace QuickFrame.Common
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DbConfig
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public DbConnString GetConnString() => EnableConnName switch
        {
            nameof(DbConnectionConfig.MsSQLLocal) => DbConnStrings.MsSQLLocal,
            nameof(DbConnectionConfig.MsSQLExpress) => DbConnStrings.MsSQLExpress,
            nameof(DbConnectionConfig.MySQL) => DbConnStrings.MySQL,
            nameof(DbConnectionConfig.SQLite) => DbConnStrings.SQLite,
            _ => DbConnStrings.SQLite
        };
        /// <summary>
        /// 启用的连接名称
        /// </summary>
        public string EnableConnName { get; set; } = string.Empty;
        /// <summary>
        /// 全部连接字符串配置
        /// </summary>
        public DbConnectionConfig DbConnStrings { get; set; } = new();
    }
    /// <summary>
    /// 关系型数据库连接字符串配置
    /// </summary>
    public class DbConnectionConfig
    {
        /// <summary>
        /// mssql本地库连接
        /// </summary>
        public DbConnString MsSQLLocal { get; set; } = new();
        /// <summary>
        /// mssql远程库连接
        /// </summary>
        public DbConnString MsSQLExpress { get; set; } = new();
        /// <summary>
        /// mysql连接
        /// </summary>
        public DbConnString MySQL { get; set; } = new();
        /// <summary>
        /// sqlite连接
        /// </summary>
        public DbConnString SQLite { get; set; } = new();
    }
    /// <summary>
    /// 工作库和后台库连接串
    /// </summary>
    public class DbConnString
    {
        /// <summary>
        /// 工作库
        /// </summary>
        public string WorkDb { get; set; } = string.Empty;
        /// <summary>
        /// 后台库
        /// </summary>
        public string BackDb { get; set; } = string.Empty;
    }
}