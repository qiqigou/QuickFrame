using System;
using System.Threading.Tasks;

namespace QuickFrame.Common
{
    /// <summary>
    /// 数据缓存接口(可切换缓存介质:内存,Redis)
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Key存在则删除
        /// </summary>
        /// <param name="key">键</param>
        bool Delete(string key);
        /// <summary>
        /// Key存在则删除
        /// </summary>
        /// <param name="keys">键</param>
        int Delete(string[] keys);
        /// <summary>
        /// Key存在则删除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string key);
        /// <summary>
        /// Key存在则删除
        /// </summary>
        /// <param name="keys">键</param>
        /// <returns></returns>
        Task<int> DeleteAsync(string[] keys);
        /// <summary>
        /// 根据 key 模版删除
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        Task<int> DeleteByPatternAsync(string pattern);
        /// <summary>
        /// 检查key是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool Exists(string key);
        /// <summary>
        /// 检查key是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        string? Get(string key);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        T? Get<T>(string key);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<string?> GetAsync(string key);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<T?> GetAsync<T>(string key);
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">有效期</param>
        bool Set(string key, object value, TimeSpan? expire = default);
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">有效期</param>
        /// <returns></returns>
        Task<bool> SetAsync(string key, object value, TimeSpan? expire = default);
    }
}
