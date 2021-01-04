using QuickFrame.Common;

namespace QuickFrame.Models
{
    /// <summary>
    /// 商场表新增模型
    /// </summary>
    public class SysCompanybakInput : IDataInput
    {
        /// <summary>
        /// 商场ID
        /// </summary>
        public int ScbCorder { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        [StringColumn]
        public string ScyCcompanyid { get; set; } = string.Empty;
        /// <summary>
        /// 商场编号
        /// </summary>
        [StringColumn]
        public string ScsCshopid { get; set; } = string.Empty;
        /// <summary>
        /// 备份文件名
        /// </summary>
        [StringColumn]
        public string ScbCtype { get; set; } = string.Empty;
    }
    /// <summary>
    /// 商场表修改模型
    /// </summary>
    public class SysCompanybakUpdInput : WithStampDataInput
    {
        /// <summary>
        /// 商场编号
        /// </summary>
        [StringColumn]
        public string? ScsCshopid { get; set; }
        /// <summary>
        /// 备份文件名
        /// </summary>
        [StringColumn]
        public string? ScbCtype { get; set; }
    }
}
