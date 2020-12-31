using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFrame.Common
{
    /// <summary>
    /// 数字类型列(适用于Db模型)
    /// </summary>
    public class DecimalColumnAttribute : ColumnAttribute
    {
        public DecimalColumnAttribute()
        {
            TypeName = "decimal(18, 5)";
        }
    }
    /// <summary>
    /// 字符类型默认长度列(同时适用于Db模型与输入模型)
    /// </summary>
    public class StringColumnAttribute : MaxLengthAttribute
    {
        public StringColumnAttribute() : base(40)
        {
            ErrorMessage = "{0}长度不能超过{1}";
        }

        public StringColumnAttribute(int length) : base(length)
        {
            ErrorMessage = "{0}长度不能超过{1}";
        }
    }
    /// <summary>
    /// 备注类型列(同时适用于Db模型与输入模型)
    /// </summary>
    public class RemarkColumnAttribute : MaxLengthAttribute
    {
        public RemarkColumnAttribute() : base(255)
        {
            ErrorMessage = "{0}长度不能超过{1}";
        }
    }
    /// <summary>
    /// 路径类型列(同时适用于Db模型与输入模型)
    /// </summary>
    public class PathColumnAttribute : MaxLengthAttribute
    {
        public PathColumnAttribute() : base(500)
        {
            ErrorMessage = "{0}长度不能超过{1}";
        }
    }
    /// <summary>
    /// 文本类型列(同时适用于Db模型与输入模型)
    /// </summary>
    public class TextColumnAttribute : MaxLengthAttribute
    {
        public TextColumnAttribute() : base(1000)
        {
            ErrorMessage = "{0}长度不能超过{1}";
        }
    }
    /// <summary>
    /// 限制字符长度，并设置统一的提示文本(适用于输入模型)
    /// </summary>
    public class StringLengthInputAttribute : StringLengthAttribute
    {
        public StringLengthInputAttribute(int maximumLength) : base(maximumLength)
        {
            ErrorMessage = "{0}长度在{2}~{1}之间";
        }
    }
    /// <summary>
    /// 指定Flag类型(适用于Db模型)
    /// </summary>
    public class FlagColumnAttribute : ColumnAttribute
    {
        public FlagColumnAttribute()
        {
            TypeName = "char(1)";
        }
    }
    /// <summary>
    /// 标志类型(适用于输入模型)
    /// </summary>
    public class FlagInputAttribute : RegularExpressionAttribute
    {
        public FlagInputAttribute() : base("^0|1$")
        {
            ErrorMessage = "{0}值为0或1";
        }
    }
}
