using System.ComponentModel;
using System.Reflection;

namespace System
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            string name = value.ToString();
            var desc = value.GetType().GetField(name)?.GetCustomAttribute<DescriptionAttribute>();
            return desc?.Description ?? name;
        }
        /// <summary>
        /// 值转string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ValueToString(this Enum value) => ((int)Enum.Parse(value.GetType(), value.ToString())).ToString();
    }
}
