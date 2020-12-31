using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 判断类型是否为可空(Nullable)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type? type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
        /// <summary>
        /// 判断类型是否为匿名类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAnonymousType(this Type? type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }
    }
}
