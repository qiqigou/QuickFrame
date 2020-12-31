using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using QuickFrame.Common;

namespace QuickFrame.Model
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
            if (conn.Type == DbType.MSSQL)
            {
                optionsBuilder.UseSqlServer(builder.ConnectionString);
            }
            else
            {
                optionsBuilder.UseSqlite(builder.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var types = Assembly.Load(AssemblyOption.ModelName).GetTypes().Where(x => x.IsAssignableTo(typeof(IDbEntity<WorkOption>))).ToArray();
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
