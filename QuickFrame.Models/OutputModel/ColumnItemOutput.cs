using QuickFrame.Common;

namespace QuickFrame.Models
{
    /// <summary>
    /// 列输出模型
    /// </summary>
    public class ColumnItemOutput : IMEntity
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; } = string.Empty;
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 列宽
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 列类型
        /// </summary>
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// 隐藏
        /// </summary>
        public bool Hide { get; set; }
    }
}
