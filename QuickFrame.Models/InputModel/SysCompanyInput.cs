using QuickFrame.Common;

namespace QuickFrame.Models
{
    /// <summary>
    /// 公司表新增模型
    /// </summary>
    public class SysCompanyInput : IDataInput
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        [StringColumn]
        public string ScyCcompanyid { get; set; } = string.Empty;
        /// <summary>
        /// 公司名称
        /// </summary>
        [StringColumn]
        public string ScyCcompanyname { get; set; } = string.Empty;
        /// <summary>
        /// 公司负责人
        /// </summary>
        [StringColumn]
        public string ScyCman { get; set; } = string.Empty;
        /// <summary>
        /// 公司地址
        /// </summary>
        [StringColumn]
        public string ScyCadr { get; set; } = string.Empty;
        /// <summary>
        /// 公司电话
        /// </summary>
        [StringColumn]
        public string ScyCtel { get; set; } = string.Empty;
        /// <summary>
        /// 公司传真
        /// </summary>
        [StringColumn]
        public string ScyCfax { get; set; } = string.Empty;
        /// <summary>
        /// 公司邮箱
        /// </summary>
        [StringColumn]
        public string ScyCemail { get; set; } = string.Empty;
        /// <summary>
        /// 状态(1有效,0失效)
        /// </summary>
        [FlagInput]
        public string ScyCstate { get; set; } = string.Empty;
        /// <summary>
        /// 公司网站
        /// </summary>
        [StringColumn]
        public string ScyCweb { get; set; } = string.Empty;
    }
    /// <summary>
    /// 公司表修改模型
    /// </summary>
    public class SysCompanyUpdInput : WithStampDataInput
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [StringColumn]
        public string? ScyCcompanyname { get; set; }
        /// <summary>
        /// 公司负责人
        /// </summary>
        [StringColumn]
        public string? ScyCman { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        [StringColumn]
        public string? ScyCadr { get; set; }
        /// <summary>
        /// 公司电话
        /// </summary>
        [StringColumn]
        public string? ScyCtel { get; set; }
        /// <summary>
        /// 公司传真
        /// </summary>
        [StringColumn]
        public string? ScyCfax { get; set; }
        /// <summary>
        /// 公司邮箱
        /// </summary>
        [StringColumn]
        public string? ScyCemail { get; set; }
        /// <summary>
        /// 状态(1有效,0失效)
        /// </summary>
        [FlagInput]
        public string? ScyCstate { get; set; }
        /// <summary>
        /// 公司网站
        /// </summary>
        [StringColumn]
        public string? ScyCweb { get; set; }
    }
}
