using QuickFrame.Common;

namespace QuickFrame.Models
{
    /// <summary>
    /// 新增字段过滤规则
    /// </summary>
    public class FieldFilterInput : IDataInput
    {
        /// <summary>
        /// 源类型名称
        /// </summary>
        [StringColumn]
        public string FgSource { get; set; } = string.Empty;
        /// <summary>
        /// 目标类型名称
        /// </summary>
        [StringColumn]
        public string FgDest { get; set; } = string.Empty;
    }
    /// <summary>
    /// 修改字段过滤规则
    /// </summary>
    public class FieldFilterUpdInput : IDataInput
    {
        /// <summary>
        /// 源类型名称
        /// </summary>
        [StringColumn]
        public string? FgSource { get; set; }
        /// <summary>
        /// 目标类型名称
        /// </summary>
        [StringColumn]
        public string? FgDest { get; set; }
    }
    /// <summary>
    /// 新增时字段过滤子表
    /// </summary>
    public class FieldFiltercInput : IDataInput
    {
        /// <summary>
        /// ID
        /// </summary>
        public int FgcIorder { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        [StringColumn]
        public string FgcField { get; set; } = string.Empty;
    }
    /// <summary>
    /// 修改时字段过滤子表
    /// </summary>
    public class FieldFiltercUpdInput : IDataInput
    {
        /// <summary>
        /// ID
        /// </summary>
        public int FgcIorder { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        [StringColumn]
        public string? FgcField { get; set; }
    }
}
