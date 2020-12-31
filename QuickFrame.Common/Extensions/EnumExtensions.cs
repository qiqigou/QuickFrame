using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using QuickFrame.Common;

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
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum item)
        {
            string name = item.ToString();
            var desc = item.GetType().GetField(name)?.GetCustomAttribute<DescriptionAttribute>();
            return desc?.Description ?? name;
        }
        /// <summary>
        /// 转为long类型
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static long ToInt64(this Enum item)
        {
            return Convert.ToInt64(item);
        }
        /// <summary>
        /// 转为Options
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<OptionOutput> ToList(this Enum value)
        {
            var enumType = value.GetType();
            if (!enumType.IsEnum)
                return new List<OptionOutput>(0);
            return Enum.GetValues(enumType).Cast<Enum>().Select(x => new OptionOutput
            {
                Label = x.GetDescription(),
                Value = x
            }).ToList();
        }
        /// <summary>
        /// 转为Options
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<OptionOutput> ToList<T>()
            where T : Enum
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                return new List<OptionOutput>(0);
            return Enum.GetValues(enumType).Cast<Enum>().Select(x => new OptionOutput
            {
                Label = x.GetDescription(),
                Value = x
            }).ToList();
        }
    }
}
