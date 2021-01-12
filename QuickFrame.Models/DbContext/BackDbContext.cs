using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuickFrame.Common;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Models
{
    /// <summary>
    /// 后台库上下文
    /// </summary>
    public class BackDbContext : DbContext
    {
        private readonly DbConfig _dbConfig;

        public BackDbContext(IOptions<DbConfig> options)
        {
            _dbConfig = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(ConsoleloggerFactoryToSql);
            var conn = _dbConfig.GetConnString();
            _ = _dbConfig.EnableConnName switch
            {
                nameof(DbConnectionConfig.MsSQLLocal) => optionsBuilder.UseSqlServer(conn.BackDb),
                nameof(DbConnectionConfig.MsSQLExpress) => optionsBuilder.UseSqlServer(conn.BackDb),
                nameof(DbConnectionConfig.MySQL) => optionsBuilder.UseMySql(conn.BackDb, ServerVersion.FromString("8.0.22")),
                nameof(DbConnectionConfig.SQLite) => optionsBuilder.UseSqlite(conn.BackDb),
                _ => optionsBuilder.UseSqlite(conn.BackDb),
            };
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var types = Assembly.Load(AssemblyOption.ModelsName).GetTypes().Where(x => x.IsAssignableTo(typeof(IDbEntity<BackOption>))).ToArray();
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
        /// <summary>
        /// 在控制台输出SQL语句
        /// </summary>
        public static readonly ILoggerFactory ConsoleloggerFactoryToSql =
            LoggerFactory.Create(builder =>
            {
                builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).AddConsole();
            });
    }
}
