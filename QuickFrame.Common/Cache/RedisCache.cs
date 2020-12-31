using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuickFrame.Common
{
    /// <summary>
    /// 数据缓存(基于Redis)
    /// </summary>
    public class RedisCache : ICache
    {
        public bool Delete(string key)
        {
            return RedisHelper.Del(key) > 0;
        }

        public int Delete(string[] keys)
        {
            return (int)RedisHelper.Del(keys);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await RedisHelper.DelAsync(key) > 0;
        }

        public async Task<int> DeleteAsync(string[] keys)
        {
            return (int)await RedisHelper.DelAsync(keys);
        }

        public async Task<int> DeleteByPatternAsync(string pattern)
        {
            if (pattern.IsNull())
                return default;
            pattern = Regex.Replace(pattern, @"\{.*\}", "*");
            var keys = (await RedisHelper.KeysAsync(pattern));
            if (keys != null && keys.Length > 0)
            {
                return (int)await RedisHelper.DelAsync(keys);
            }
            return default;
        }

        public bool Exists(string key)
        {
            return RedisHelper.Exists(key);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return RedisHelper.ExistsAsync(key);
        }

        public string? Get(string key)
        {
            return RedisHelper.Get(key);
        }

        public T? Get<T>(string key)
        {
            return RedisHelper.Get<T>(key);
        }

        public Task<string?> GetAsync(string key)
        {
            return RedisHelper.GetAsync(key);
        }

        public Task<T?> GetAsync<T>(string key)
        {
            return RedisHelper.GetAsync<T?>(key);
        }

        public bool Set(string key, object value, TimeSpan? expire = default)
        {
            if (expire.HasValue)
            {
                return RedisHelper.Set(key, value, expire.Value);
            }
            else
            {
                //redis缓存默认设置10分钟过期
                return RedisHelper.Set(key, value, TimeSpan.FromMinutes(10));
            }
        }

        public Task<bool> SetAsync(string key, object value, TimeSpan? expire = default)
        {
            if (expire.HasValue)
            {
                return RedisHelper.SetAsync(key, value, expire.Value);
            }
            else
            {
                //redis缓存默认设置10分钟过期
                return RedisHelper.SetAsync(key, value, TimeSpan.FromMinutes(10));
            }
        }
    }
}
