using QuickFrame.Common;

namespace QuickFrame.Models
{
    /// <summary>
    /// 专卖店表新增模型
    /// </summary>
    public class SysCompanyshopInput : IDataInput
    {
        /// <summary>
        /// 商场编号
        /// </summary>
        [StringColumn]
        public string ScsCshopid { get; set; } = string.Empty;
        /// <summary>
        /// 公司编号
        /// </summary>
        [StringColumn]
        public string ScyCcompanyid { get; set; } = string.Empty;
        /// <summary>
        /// 商场名称
        /// </summary>
        [StringColumn]
        public string ScyCshopname { get; set; } = string.Empty;
        /// <summary>
        /// 是否为主商场
        /// </summary>
        [FlagInput]
        public string ScyCmcflag { get; set; } = string.Empty;
        /// <summary>
        /// 商场负责人
        /// </summary>
        [StringColumn]
        public string ScyCman { get; set; } = string.Empty;
        /// <summary>
        /// 商场地址
        /// </summary>
        [StringColumn]
        public string ScyCadr { get; set; } = string.Empty;
        /// <summary>
        /// 商场电话
        /// </summary>
        [StringColumn]
        public string ScyCtel { get; set; } = string.Empty;
        /// <summary>
        /// 商场传真
        /// </summary>
        [StringColumn]
        public string ScyCfax { get; set; } = string.Empty;
        /// <summary>
        /// 商场邮箱
        /// </summary>
        [StringColumn]
        public string ScyCemail { get; set; } = string.Empty;
        /// <summary>
        /// 状态(1有效,0失效)
        /// </summary>
        [FlagInput]
        public string ScyCstate { get; set; } = string.Empty;
        /// <summary>
        /// 商场网站
        /// </summary>
        [StringColumn]
        public string ScyCweb { get; set; } = string.Empty;
    }
    /// <summary>
    /// 专卖店修改模型
    /// </summary>
    public class SysCompanyshopUpdInput : WithStampDataInput
    {
        /// <summary>
        /// 商场名称
        /// </summary>
        [StringColumn]
        public string? ScyCshopname { get; set; }
        /// <summary>
        /// 是否为主商场
        /// </summary>
        [FlagInput]
        public string? ScyCmcflag { get; set; }
        /// <summary>
        /// 商场负责人
        /// </summary>
        [StringColumn]
        public string? ScyCman { get; set; }
        /// <summary>
        /// 商场地址
        /// </summary>
        [StringColumn]
        public string? ScyCadr { get; set; }
        /// <summary>
        /// 商场电话
        /// </summary>
        [StringColumn]
        public string? ScyCtel { get; set; }
        /// <summary>
        /// 商场传真
        /// </summary>
        [StringColumn]
        public string? ScyCfax { get; set; }
        /// <summary>
        /// 商场邮箱
        /// </summary>
        [StringColumn]
        public string? ScyCemail { get; set; }
        /// <summary>
        /// 状态(1有效,0失效)
        /// </summary>
        [FlagInput]
        public string? ScyCstate { get; set; }
        /// <summary>
        /// 商场网站
        /// </summary>
        [StringColumn]
        public string? ScyCweb { get; set; }
    }
}
