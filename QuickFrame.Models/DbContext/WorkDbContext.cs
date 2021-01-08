using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuickFrame.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Models
{
    /// <summary>
    /// 业务库上下文
    /// </summary>
    public class WorkDbContext : DbContext
    {
        private readonly DbConfig _dbConfig;
        private readonly IUser _user;

        public WorkDbContext(IOptions<DbConfig> options, IUser user)
        {
            _dbConfig = options.Value;
            _user = user;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = _dbConfig.GetWorkString();
            var builder = new SqlConnectionStringBuilder(conn.ConnectionString);
            if (_user.DBName.NotNull())
            {
                builder.InitialCatalog = _user.DBName;
            }
            _ = conn.Type switch
            {
                DbType.MSSQL => optionsBuilder.UseSqlServer(builder.ConnectionString),
                DbType.SQLite => optionsBuilder.UseSqlite(builder.ConnectionString),
                DbType.MYSQL => optionsBuilder.UseMySql(builder.ConnectionString, ServerVersion.FromString("8.0.22")),
                _ => throw new AmbiguousMatchException($"{conn.Type}不是受支持的数据库类型")
            };
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var types = Assembly.Load(AssemblyOption.ModelsName).GetTypes().Where(x => x.IsAssignableTo(typeof(IDbEntity<WorkOption>))).ToArray();
            var tables = types.Where(x => x.IsAssignableTo(typeof(TableEntity))).ToArray();
            foreach (var item in tables)
            {
                var entity = modelBuilder.Entity(item);
                var keys = item.GetProperties().Where(x => x.GetCustomAttribute<KeyAttribute>() != default).Select(x => x.Name).ToArray();
                if (keys.Length > 1)
                {
                    entity.HasKey(keys);
                }
            }
            var views = types.Where(x => x.IsAssignableTo(typeof(ViewEntity))).ToArray();
            foreach (var item in views)
            {
                modelBuilder.Entity(item).HasNoKey().ToView(item.Name);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
