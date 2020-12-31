using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Threading.Tasks;

namespace QuickFrame.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())//替换asp.net core默认的容器
            .UseSerilog((host, config) =>
            {
                config.ReadFrom.Configuration(host.Configuration);
                var filepath = host.Configuration.GetValue<string>("Serilog:FilePath");//日志输出位置
                var rolling = host.Configuration.GetValue<RollingInterval>("Serilog:RollingInterval");//日志滚动间隔(值是一个枚举,3标识每天一个日志文件)
                var fileCountLimit = host.Configuration.GetValue<int>("Serilog:FileCountLimit");//日志保留天数
                config.WriteTo.File(filepath,
                    rollingInterval: rolling,
                    retainedFileCountLimit: fileCountLimit,
                    outputTemplate: host.Configuration.GetValue<string>("Serilog:FileTemplate"));
                config.WriteTo.Console(outputTemplate: host.Configuration.GetValue<string>("Serilog:ConsoleTemplate"));
            })
            .ConfigureLogging(logging => logging.ClearProviders())//清空asp.net core自带的日志提供程序
            .ConfigureAppConfiguration((hostingContext, config) =>//注入自定义的配置文件
            {
                var env = hostingContext.HostingEnvironment;
                config.AddJsonFile("configs/jwtconfig.json", true, true);
                config.AddJsonFile("configs/appconfig.json", true, true);
                config.AddJsonFile("configs/dbconfig.json", true, true);
                config.AddJsonFile("configs/logconfig.json", true, true);
                config.AddJsonFile("configs/cacheconfig.json", true, true);
                if (env.IsDevelopment())
                {
                    config.AddJsonFile($"configs/jwtconfig.{env.EnvironmentName}.json", true, true);
                    config.AddJsonFile($"configs/appconfig.{env.EnvironmentName}.json", true, true);
                    config.AddJsonFile($"configs/dbconfig.{env.EnvironmentName}.json", true, true);
                    config.AddJsonFile($"configs/logconfig.{env.EnvironmentName}.json", true, true);
                    config.AddJsonFile($"configs/cacheconfig.{env.EnvironmentName}.json", true, true);
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
