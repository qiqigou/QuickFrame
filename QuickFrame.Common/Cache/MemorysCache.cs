using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace QuickFrame.Common
{
    /// <summary>
    /// 数据缓存(基于内存)
    /// </summary>
    public class MemorysCache : ICache
    {
        private readonly IMemoryCache _memoryCache;
        public MemorysCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool Delete(string key)
        {
            _memoryCache.Remove(key);
            return true;
        }

        public int Delete(string[] keys)
        {
            foreach (var k in keys)
            {
                _memoryCache.Remove(k);
            }
            return keys.Length;
        }

        public Task<bool> DeleteAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.FromResult(true);
        }

        public Task<int> DeleteAsync(string[] keys)
        {
            foreach (var k in keys)
            {
                _memoryCache.Remove(k);
            }

            return Task.FromResult(keys.Length);
        }

        public async Task<int> DeleteByPatternAsync(string pattern)
        {
            if (pattern.IsNull())
                return default;
            pattern = Regex.Replace(pattern, @"\{.*\}", "(.*)");
            var keys = GetAllKeys().Where(k => Regex.IsMatch(k, pattern));
            if (keys != default && keys.Any())
            {
                return await DeleteAsync(keys.ToArray());
            }
            return default;
        }

        public bool Exists(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(_memoryCache.TryGetValue(key, out _));
        }

        public string? Get(string key)
        {
            return _memoryCache.Get(key)?.ToString() ?? string.Empty;
        }

        public T? Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public Task<string?> GetAsync(string key)
        {
            return Task.FromResult(Get(key));
        }

        public Task<T?> GetAsync<T>(string key)
        {
            return Task.FromResult(Get<T>(key));
        }

        public bool Set(string key, object value, TimeSpan? expire = default)
        {
            if (expire.HasValue)
            {
                _memoryCache.Set(key, value, expire.Value);
            }
            else
            {
                //内存缓存不需要设置默认过期时间,程序停止缓存即失效
                _memoryCache.Set(key, value);
            }
            return true;
        }

        public Task<bool> SetAsync(string key, object value, TimeSpan? expire = default)
        {
            if (expire.HasValue)
            {
                Set(key, value, expire.Value);
            }
            else
            {
                //内存缓存不需要设置默认过期时间,程序停止缓存即失效
                Set(key, value);
            }
            return Task.FromResult(true);
        }

        private List<string> GetAllKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _memoryCache.GetType()?.GetField("_entries", flags)?.GetValue(_memoryCache);
            if (entries is not IDictionary cacheItems) return new List<string>(0);
            var keys = new List<string>();
            foreach (DictionaryEntry? cacheItem in cacheItems)
            {
                if (cacheItem.HasValue)
                {
                    var key = cacheItem?.Key.ToString();
                    if (key != default)
                        keys.Add(key);
                }
            }
            return keys;
        }
    }
}
