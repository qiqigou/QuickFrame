using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuickFrame.Common;

namespace QuickFrame.Models
{
    /// <summary>
    /// 专卖店
    /// </summary>
    public class syscompanyshop_scs : WithStampTable, IDbEntity<BackOption>
    {
        /// <summary>
        /// 商场编号
        /// </summary>
        [Key]
        [StringColumn]
        public string scs_cshopid { get; set; } = string.Empty;
        /// <summary>
        /// 公司编号
        /// </summary>
        [Key]
        [StringColumn]
        public string scy_ccompanyid { get; set; } = string.Empty;
        /// <summary>
        /// 商场名称
        /// </summary>
        [StringColumn]
        public string scs_cshopname { get; set; } = string.Empty;
        /// <summary>
        /// 是否为主商场
        /// </summary>
        [FlagColumn]
        public string scs_cmcflag { get; set; } = string.Empty;
        /// <summary>
        /// 商场负责人
        /// </summary>
        [StringColumn]
        public string scs_cman { get; set; } = string.Empty;
        /// <summary>
        /// 商场地址
        /// </summary>
        [StringColumn]
        public string scs_cadr { get; set; } = string.Empty;
        /// <summary>
        /// 商场电话
        /// </summary>
        [StringColumn]
        public string scs_ctel { get; set; } = string.Empty;
        /// <summary>
        /// 商场传真
        /// </summary>
        [StringColumn]
        public string scs_cfax { get; set; } = string.Empty;
        /// <summary>
        /// 商场邮箱
        /// </summary>
        [StringColumn]
        public string scs_cemail { get; set; } = string.Empty;
        /// <summary>
        /// 状态(1有效,0失效)
        /// </summary>
        [FlagColumn]
        public string scs_cstate { get; set; } = string.Empty;
        /// <summary>
        /// 商场网站
        /// </summary>
        [StringColumn]
        public string scs_cweb { get; set; } = string.Empty;
        /// <summary>
        /// 商场
        /// </summary>
        [ForeignKey(nameof(scy_ccompanyid))]
        public syscompany_scy? syscompany_scy { get; set; }

    }
}
