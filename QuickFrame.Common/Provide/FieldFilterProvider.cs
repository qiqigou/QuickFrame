using System;
using System.Collections.Generic;
using System.Threading;
using DynamicTypeMap;
using Microsoft.Extensions.DependencyInjection;

namespace QuickFrame.Common
{
    /// <summary>
    /// 字段过滤提供者
    /// </summary>
    [SingletonInjection]
    public class FieldFilterProvider : IFieldFilterProvider
    {
        private readonly Dictionary<DyMapValue, DynamicTypeConfig> _map;
        private readonly IServiceProvider _provider;
        private readonly IUser _user;
        private readonly ReaderWriterLockSlim _rwLock;

        public FieldFilterProvider(IServiceProvider provider, IUser user)
        {
            _map = new Dictionary<DyMapValue, DynamicTypeConfig>(5);
            _provider = provider;
            _user = user;
            _rwLock = new ReaderWriterLockSlim();
        }
        /// <summary>
        /// 新类型的默认名称
        /// </summary>
        /// <param name="sourceTypeName"></param>
        /// <returns></returns>
        private static string DefaultName(string sourceTypeName) => $"{sourceTypeName}_DTO";
        /// <summary>
        /// 获取类型的缓存配置
        /// </summary>
        private DynamicTypeConfig Config
        {
            get
            {
                _rwLock.EnterUpgradeableReadLock();
                try
                {
                    var key = new DyMapValue(_user.DBName, _user.Role);
                    if (_map.TryGetValue(key, out var mp))
                    {
                        return mp;
                    }
                    else
                    {
                        try
                        {
                            _rwLock.EnterWriteLock();
                            return _map[key] = new DynamicTypeConfig();
                        }
                        finally
                        {
                            _rwLock.ExitWriteLock();
                        }
                    }
                }
                finally
                {
                    _rwLock.ExitUpgradeableReadLock();
                }
            }
        }
        /// <summary>
        /// 取得数据适配器(用于获取动态设置的字段)
        /// </summary>
        private Func<IDataAdapter> DataAdapter => () => _provider.GetRequiredService<IDataAdapter>();
        /// <summary>
        /// 执行过滤
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="data"></param>
        /// <param name="newTypeName"></param>
        /// <returns></returns>
        public object? Filter<TModel>(TModel? data, string? newTypeName = default)
            where TModel : class
        {
            newTypeName ??= DefaultName(typeof(TModel).FullName ?? string.Empty);
            if (Config.GetOrCreateNewType(typeof(TModel), newTypeName, DataAdapter).MapFunc is Func<TModel, object> func)
            {
                if (data != default) return func.Invoke(data);
            }
            return data;
        }
        /// <summary>
        /// 执行过滤(集合)
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="data"></param>
        /// <param name="newTypeName"></param>
        /// <returns></returns>
        public IEnumerable<object>? FilterRange<TModel>(IEnumerable<TModel?>? data, string? newTypeName = default)
            where TModel : class
        {
            if (data == default) goto end;
            newTypeName ??= DefaultName(typeof(TModel).FullName ?? string.Empty);
            if (Config.GetOrCreateNewType(typeof(TModel), newTypeName, DataAdapter).MapFunc is Func<TModel, object> func)
            {
                foreach (var item in data)
                {
                    if (item != default) yield return func?.Invoke(item) ?? item;
                }
            }
        end:;
        }
        /// <summary>
        /// 移除缓存中的新类型
        /// </summary>
        /// <param name="sourceTypeName"></param>
        /// <param name="newTypeName"></param>
        /// <returns></returns>
        public bool RemoveNewType(string sourceTypeName, string? newTypeName = default)
        {
            newTypeName ??= DefaultName(sourceTypeName);
            return Config.RemoveNewType(sourceTypeName, newTypeName);
        }
    }
    /// <summary>
    /// 区分库与角色
    /// </summary>
    /// <remarks>
    /// 由于字段设置在业务库中，而业务库是可以随时切换的.
    /// 所以不同的库和不同的角色都有不同的配置.
    /// </remarks>
    internal struct DyMapValue
    {
        public string DBName { get; set; }
        public string RoleName { get; set; }

        public DyMapValue(string dbname, string rolename)
        {
            DBName = dbname;
            RoleName = rolename;
        }
    }
}