using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuickFrame.Common;

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
            optionsBuilder.UseLoggerFactory(BackDbContext.ConsoleloggerFactoryToSql);
            var conn = _dbConfig.GetConnString();
            var builder = new SqlConnectionStringBuilder(conn.WorkDb);
            if (_user.DBName.NotNull())
            {
                builder.InitialCatalog = _user.DBName;
            }
            _ = _dbConfig.EnableConnName switch
            {
                nameof(DbConnectionConfig.MsSQLLocal) => optionsBuilder.UseSqlServer(conn.WorkDb),
                nameof(DbConnectionConfig.MsSQLExpress) => optionsBuilder.UseSqlServer(conn.WorkDb),
                nameof(DbConnectionConfig.MySQL) => optionsBuilder.UseMySql(conn.WorkDb, ServerVersion.FromString("8.0.22")),
                nameof(DbConnectionConfig.SQLite) => optionsBuilder.UseSqlite(conn.WorkDb),
                _ => optionsBuilder.UseSqlite(conn.WorkDb),
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
