using QuickFrame.Common;

namespace QuickFrame.Model
{
    public class v_fieldfilterc : ViewEntity, IDbEntity<WorkOption>
    {
        /// <summary>
        /// 主表ID
        /// </summary>
        public long fg_id { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public int fgc_iorder { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string fgc_field { get; set; } = string.Empty;
    }
}
