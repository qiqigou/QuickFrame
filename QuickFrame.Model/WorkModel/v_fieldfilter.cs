namespace QuickFrame.Model
{
    public class v_fieldfilter : WithStampView, IDbEntity<WorkOption>
    {
        /// <summary>
        /// id
        /// </summary>
        public long fg_id { get; set; }
        /// <summary>
        /// 源类型名称
        /// </summary>
        public string fg_source { get; set; } = string.Empty;
        /// <summary>
        /// 目标类型名称
        /// </summary>
        public string fg_dest { get; set; } = string.Empty;
    }
}
