using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickFrame.Common
{
    /// <summary>
    /// 查询时过滤条件
    /// </summary>
    public class ObjFilterInput : IDataInput
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public GroupInput? Condition { get; set; }
        /// <summary>
        /// 分页信息
        /// </summary>
        public PageInput Page { get; set; } = new PageInput();
    }
    /// <summary>
    /// 条件分组
    /// </summary>
    public class GroupInput : IDataInput
    {
        /// <summary>
        /// 分组中的逻辑运算符
        /// </summary>
        [RegularExpression("^and|or$")]
        public string Logic { get; set; } = ConstantOptions.LogicConstant.And;
        /// <summary>
        /// 条件分组
        /// </summary>
        public ItemInput[]? Items { get; set; }
        /// <summary>
        /// 支持Group嵌套
        /// </summary>
        [JsonIgnore]
        public GroupInput[]? Groups { get; set; }
    }
    /// <summary>
    /// 条件项
    /// </summary>
    public class ItemInput : IDataInput
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        [StringColumn]
        public string Field { get; set; } = string.Empty;
        /// <summary>
        /// 字段值
        /// </summary>
        [TextColumn]
        public string Value { get; set; } = string.Empty;
        /// <summary>
        /// 比较符
        /// </summary>
        [RegularExpression("^equal|notequal|less|greater|lesseq|greatereq|contains$")]
        public string Compare { get; set; } = ConstantOptions.CompareConstant.Contains;
    }
}
