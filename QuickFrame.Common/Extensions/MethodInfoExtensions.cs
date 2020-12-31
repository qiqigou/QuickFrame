using System.Linq;
using System.Threading.Tasks;

namespace System.Reflection
{
    /// <summary>
    /// 反射扩展(MethodInfo)
    /// </summary>
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// 是否有特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool HasAttribute<T>(this MethodInfo method)
        {
            return method.GetCustomAttributes(typeof(T), false).FirstOrDefault() is T;
        }
        /// <summary>
        /// 获取特性对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public static T? GetAttribute<T>(this MethodInfo method)
            where T : Attribute
        {
            return method.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }
        /// <summary>
        /// 是否是异步方法
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool IsAsync(this MethodInfo method)
        {
            return method.ReturnType == typeof(Task)
                || (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>));
        }
    }
}
