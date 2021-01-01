using QuickFrame.Common;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Model
{
    /// <summary>
    /// 层次-权限表
    /// </summary>
	public class syspopelist_pl : WithStampTable, IDbEntity<BackOption>
    {
        /// <summary>
        /// 功能层次码
        /// </summary>
        [Key]
        [StringColumn]
        public string pl_clevel { get; set; } = string.Empty;
        /// <summary>
        /// 权限描述
        /// </summary>
        [RemarkColumn]
        public string pl_cfuncdes { get; set; } = string.Empty;
    }
}
