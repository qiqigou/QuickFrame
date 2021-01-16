using System;
using System.Net.Http;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuickFrame.Common;
using QuickFrame.Web;

namespace QuickFrame.Tests
{
    /// <summary>
    /// 测试基类
    /// </summary>
    public class BaseTest
    {
        protected readonly TestServer _server;
        protected readonly HttpClient _client;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly AppConfig _appConfig;
        protected readonly IConfiguration _configuration;

        public T GetService<T>()
            => _serviceProvider.GetService<T>() ?? throw new ArgumentNullException(nameof(T));
        public T GetRequiredService<T>() where T : notnull
            => _serviceProvider.GetRequiredService<T>();
        /// <summary>
        /// 测试基类
        /// </summary>
        protected BaseTest()
        {
            var builder = CreateHostBuilder();
            var host = builder.Build();
            host.Start();

            _server = host.GetTestServer();
            _client = host.GetTestClient();
            _serviceProvider = _server.Services;
            _configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            _appConfig = _configuration.Get<AppConfig>();
        }
        /// <summary>
        /// 创建主机
        /// </summary>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder()
        {
            var configBilder = new ConfigurationBuilder();
            configBilder.AddJsonFile("appsettings.json", true, true);
            configBilder.AddJsonFile("configs/jwtconfig.json", true, true);
            configBilder.AddJsonFile("configs/appconfig.json", true, true);
            configBilder.AddJsonFile("configs/dbconfig.json", true, true);
            configBilder.AddJsonFile($"configs/cacheconfig.json", true, true);
#if DEBUG
            configBilder.AddJsonFile($"appsettings.{Environments.Development}.json", true, true);
            configBilder.AddJsonFile($"configs/jwtconfig.{Environments.Development}.json", true, true);
            configBilder.AddJsonFile($"configs/appconfig.{Environments.Development}.json", true, true);
            configBilder.AddJsonFile($"configs/dbconfig.{Environments.Development}.json", true, true);
            configBilder.AddJsonFile($"configs/cacheconfig.{Environments.Development}.json", true, true);
#endif
            var config = configBilder.Build();
            config.GetSection("IdentityServer:Enable").Value = "false";//禁用IdentityServer4
            return Host.CreateDefaultBuilder()
#if DEBUG
                  .UseEnvironment(Environments.Development)
#endif
                  .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder.UseConfiguration(config);
                      webBuilder.UseStartup<Startup>();
                      webBuilder.UseTestServer();
                  });
        }
    }
}
