using System.ComponentModel.DataAnnotations;
using QuickFrame.Common;

namespace QuickFrame.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class sysuserlist_ul : WithStampTable, IDbEntity<BackOption>
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        [Key]
        [StringColumn]
        public string ul_cid { get; set; } = string.Empty;
        /// <summary>
        /// 用户名称
        /// </summary>
        [StringColumn]
        public string ul_cname { get; set; } = string.Empty;
        /// <summary>
        /// 密码
        /// </summary>
        [StringColumn]
        public string ul_cpwd { get; set; } = string.Empty;
        /// <summary>
        /// 停用标识
        /// </summary>
        [StringColumn]
        public string ul_cstop { get; set; } = string.Empty;
        /// <summary>
        /// 员工编码
        /// </summary>
        [StringColumn]
        public string ul_ceeid { get; set; } = string.Empty;
        /// <summary>
        /// 备注
        /// </summary>
        [StringColumn]
        public string ul_cremark { get; set; } = string.Empty;
        /// <summary>
        /// 最低折率
        /// </summary>
        [DecimalColumn]
        public decimal ul_dagio { get; set; }
        /// <summary>
        /// 分类编码
        /// </summary>
        [StringColumn]
        public string cd_cclassid { get; set; } = string.Empty;
        /// <summary>
        /// 分类
        /// </summary>
        //[ForeignKey(nameof(cd_cclassid))]
        public sysclassdtl_cd? sysclassdtl_cd { get; set; }
        /// <summary>
        /// 分类基础表ID
        /// </summary>
        public int cs_iid { get; set; }
        /// <summary>
        /// 分类基础
        /// </summary>
        //[ForeignKey(nameof(cs_iid))]
        public sysclassset_cs? sysclassset_cs { get; set; }
    }
}
