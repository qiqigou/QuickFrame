using System;
using System.Linq;

namespace QuickFrame.Common
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DbConfig
    {
        /// <summary>
        /// 获取业务库连接
        /// </summary>
        /// <returns></returns>
        public DbConnectionConfig GetWorkString() => ConnStrings.Single(x => x.Name == Work);
        /// <summary>
        /// 获取后台库连接
        /// </summary>
        /// <returns></returns>
        public DbConnectionConfig GetBackString() => ConnStrings.Single(x => x.Name == Back);
        /// <summary>
        /// 业务库使用的数据库
        /// </summary>
        public string Work { get; set; } = string.Empty;
        /// <summary>
        /// 后台管理使用的数据库
        /// </summary>
        public string Back { get; set; } = string.Empty;
        /// <summary>
        /// 连接字符串
        /// </summary>
        public DbConnectionConfig[] ConnStrings { get; set; } = Array.Empty<DbConnectionConfig>();
    }
    /// <summary>
    /// 关系型数据库连接字符串配置
    /// </summary>
    public class DbConnectionConfig
    {
        /// <summary>
        /// 连接名
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbType Type { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;
    }
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DbType : byte
    {
        MSSQL = 0,
        SQLite = 1,
    }
}