using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using QuickFrame.Common;

namespace QuickFrame.Model
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
            var conn = _dbConfig.GetBackString();
            if (conn.Type == DbType.MSSQL)
            {
                optionsBuilder.UseSqlServer(conn.ConnectionString);
            }
            else
            {
                optionsBuilder.UseSqlite(conn.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var types = Assembly.Load(AssemblyOption.ModelName).GetTypes().Where(x => x.IsAssignableTo(typeof(IDbEntity<BackOption>))).ToArray();
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
