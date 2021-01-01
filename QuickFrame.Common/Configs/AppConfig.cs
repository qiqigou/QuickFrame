using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 跨域配置
        /// </summary>
        public string[] CorUrls { get; set; } = Array.Empty<string>();
        /// <summary>
        /// 统一认证授权服务器
        /// </summary>
        public IdentityServerConfig IdentityServer { get; set; } = new IdentityServerConfig();
        /// <summary>
        /// 开启api文档
        /// </summary>
        public bool Swagger { get; set; }
        /// <summary>
        /// api文档访问范围
        /// </summary>
        public string SwaggerScope { get; set; } = string.Empty;
        /// <summary>
        /// api文档访问范围名称
        /// </summary>
        public string SwaggerScopeName { get; set; } = string.Empty;
        /// <summary>
        /// 测试项目配置
        /// </summary>
        public TestConfig Test { get; set; } = new TestConfig();
        /// <summary>
        /// 机器标识
        /// </summary>
        public long MachineId { get; set; }
        /// <summary>
        /// 数据中心ID
        /// </summary>
        public long DatacenterId { get; set; }
        /// <summary>
        /// 唯一ID的提供方式
        /// </summary>
        public IdWorkerProvidType IdWorkerProvid { get; set; }
    }
    /// <summary>
    /// 统一认证授权服务器配置
    /// </summary>
    public class IdentityServerConfig
    {
        /// <summary>
        /// 启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; } = string.Empty;
        /// <summary>
        /// Api名称
        /// </summary>
        public string ApiName { get; set; } = string.Empty;
        /// <summary>
        /// Api密钥
        /// </summary>
        public string ApiSecret { get; set; } = string.Empty;
        /// <summary>
        /// 要求使用HTTPS
        /// </summary>
        public bool RequireHttps { get; set; }
    }
    /// <summary>
    /// 测试项目配置
    /// </summary>
    public class TestConfig
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; } = string.Empty;
        /// <summary>
        /// 业务库名称
        /// </summary>
        public string WorkDbName { get; set; } = string.Empty;
        /// <summary>
        /// 测试客户端ID
        /// </summary>
        public string ClientId { get; set; } = string.Empty;
        /// <summary>
        /// 测试客户端机密
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
        /// <summary>
        /// 访问范围
        /// </summary>
        public string Scope { get; set; } = string.Empty;
    }
    /// <summary>
    /// 唯一ID的提供方式
    /// </summary>
    public enum IdWorkerProvidType : byte
    {
        /// <summary>
        /// 雪花算法
        /// </summary>
        Snowflake = 0,
        /// <summary>
        /// Redis(需要启用Redis缓存)
        /// </summary>
        Redis = 1,
    }
}
