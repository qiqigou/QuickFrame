using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuickFrame.Common;
using QuickFrame.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace QuickFrame.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();//不过滤Claims
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;//取消Claims的默认映射

            services.Configure<JwtConfig>(_configuration);//注入jwt配置
            services.Configure<AppConfig>(_configuration);//注入app配置
            services.Configure<DbConfig>(_configuration);//注入db配置
            services.Configure<CacheConfig>(_configuration);//注入cache配置

            var jwtconfig = _configuration.Get<JwtConfig>();//获取jwt配置
            var appconfig = _configuration.Get<AppConfig>();//获取app配置
            var dbconfig = _configuration.Get<DbConfig>();//获取db配置
            var cacheconfig = _configuration.Get<CacheConfig>();//获取cache配置

            services.AddDbContextSetup();//注入数据库上下文
            services.AddCorsSetup(appconfig);//注入跨域配置
            services.AddRabbitMQSetup(appconfig);//注入RabbitMQ配置
            services.AddAuthorizationPolicySetup();//注入授权策略配置
            services.AddIdWorkerSetup(appconfig);//注入唯一ID服务
            services.AddCacheSetup(cacheconfig);//注入缓存配置
            services.AddAuthenticationSetup(appconfig, jwtconfig);//注入认证配置
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//注入身份信息解析器
            services.AddHealthChecksSetup();//注入健康检查服务
            services.AddSwaggerSetup(_environment, appconfig);//注入文档配置
            services.AddAssignProviderSetup();//注入赋值器

            //注入控制器并且设置过滤器
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiActionFilter>();
                options.Filters.Add<ApiResultFilter>();
                options.Filters.Add<ApiExceptionFilter>();
                options.MaxModelValidationErrors = 10;//模型验证最大错误数
                options.MaxValidationDepth = 20;//最大递归验证层数
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicyConfig.LowerCase;
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.Converters.Add(new TupleJsonConverter());
            })
            .AddApplicationPart(Assembly.Load(AssemblyOption.ControllersName));
        }
        /// <summary>
        /// 注入到AutoFac
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assemblyServices = Assembly.Load(AssemblyOption.ServicesName);
            var assemblyCommon = Assembly.Load(AssemblyOption.CommonName);
            var assemblyRepository = Assembly.Load(AssemblyOption.RepositorysName);

            //注入服务和仓储
            builder.AddRegisterServicesAndRepository(assemblyServices, assemblyRepository);
            //注入被特性标记的类型
            builder.AddRegisterAttributeSetup(assemblyServices, assemblyCommon, assemblyRepository);
            //注入映射器配置
            builder.AddRegisterMapsterMapper(assemblyServices, assemblyCommon, assemblyRepository);
        }
        /// <summary>
        /// 中间件管道
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//开发模式下的错误处理页面
            }
            app.UseCustomSwagger(_environment);//文档中间件
            app.UseServerIdMildd(_configuration);//服务ID中间件
            app.UseRouting();//路由中间件
            app.UseCors();//跨域中间件
            app.UseAuthentication();//认证中间件
            app.UseAuthorization();//授权中间件
            app.UseEndpoints(endpoints =>//端点中间件
            {
                endpoints.MapHealthChecks("/health");//健康检查端点
                endpoints.MapControllers();//Controllers端点
            });
        }
    }
}
