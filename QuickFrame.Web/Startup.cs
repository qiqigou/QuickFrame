using Autofac;
using Autofac.Extras.DynamicProxy;
using CSRedis;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using QuickFrame.Common;
using QuickFrame.Model;
using QuickFrame.Repository;
using QuickFrame.Service;

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
            if (_environment.IsDevelopment() || appconfig.Swagger)
            {
                services.AddCustomSwagger(appconfig);//注入文档配置
            }
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//注入身份信息解析器
            services.AddDbContext<BackDbContext>(ServiceLifetime.Scoped, ServiceLifetime.Scoped);//注入后台库
            services.AddDbContext<WorkDbContext>(ServiceLifetime.Scoped, ServiceLifetime.Scoped);//注入业务库
            services.AddCustomCors(appconfig);//注入跨域配置
            if (appconfig.IdentityServer.Enable)
            {
                services.AddCustomSSOAuthentication(appconfig);//注入SSO认证配置
            }
            else
            {
                services.AddCustomJWTAuthentication(jwtconfig);//注入JWT认证配置
            }
            services.AddSingleton<IAuthorizationHandler, RootPermissionAuthorizationHandler>();//预设角色授权策略
            services.AddScoped<IAuthorizationHandler, ApiPermissionAuthorizationHandler>();//API权限授权策略
            //注入控制器并且设置过滤器
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiActionFilter>();
                options.Filters.Add<ApiResultFilter>();
                options.Filters.Add<ApiExceptionFilter>();
                options.MaxModelValidationErrors = 10;//模型验证最大错误数
                options.MaxValidationDepth = 20;//最大递归验证层数
            })
            .AddApplicationPart(Assembly.Load(AssemblyOption.ControllersName))
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicyConfig.LowerCase;
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.Converters.Add(new TupleJsonConverter());
            });
            //注入赋值器
            services.AddSingleton(provider => provider.GetRequiredService<IAssignProvider>().CreateAssign());
            //注入健康检查服务
            services.AddHealthChecks().AddDbContextCheck<BackDbContext>().AddDbContextCheck<WorkDbContext>();
            if (cacheconfig.Type == CacheType.Redis)
            {
                //Redis缓存
                RedisHelper.Initialization(new CSRedisClient(cacheconfig.Redis.ConnectionString));
                services.AddSingleton<ICache, RedisCache>();
            }
            else
            {
                //内存缓存
                services.AddMemoryCache();
                services.AddSingleton<ICache, MemorysCache>();
            }
            //注入唯一ID服务
            if (appconfig.IdWorkerProvid == IdWorkerProvidType.Redis)
            {
                services.AddSingleton<IIdWorker, RedisIdWorker>();
            }
            else
            {
                services.AddSingleton<IIdWorker, SnowflakeIdWorker>();
            }
        }
        /// <summary>
        /// 注入到AutoFac
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assemblyServices = Assembly.Load(AssemblyOption.ServiceName);
            var assemblyCommon = Assembly.Load(AssemblyOption.CommonName);
            var assemblyRepository = Assembly.Load(AssemblyOption.RepositoryName);

            //服务注入
            builder.RegisterAssemblyTypes(assemblyServices)
            .Where(x => x.IsAssignableTo<IService>())
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .InterceptedBy(typeof(IProxyInterceptor))
            .EnableInterfaceInterceptors();//启用动态代理

            //仓储注入
            builder.RegisterAssemblyTypes(assemblyRepository)
            .Where(x => x.IsAssignableTo<IRepository>())
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

            //单例注入
            builder.RegisterAssemblyTypes(assemblyServices, assemblyCommon, assemblyRepository)
            .Where(x => x.GetCustomAttribute<SingletonInjectionAttribute>() != null)
            .AsImplementedInterfaces()
            .SingleInstance();

            //范围注入
            builder.RegisterAssemblyTypes(assemblyServices, assemblyCommon, assemblyRepository)
            .Where(x => x.GetCustomAttribute<ScopeInjectionAttribute>() != null)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

            //瞬态注入
            builder.RegisterAssemblyTypes(assemblyServices, assemblyCommon, assemblyRepository)
            .Where(x => x.GetCustomAttribute<TransientInjectionAttribute>() != null)
            .AsImplementedInterfaces()
            .InstancePerDependency();

            //映射器注入
            var config = new TypeAdapterConfig();
            config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible)
            .ShallowCopyForSameType(false)
            .IgnoreNullValues(true)
            .Config
            .Scan(assemblyServices, assemblyCommon, assemblyRepository);
            builder.RegisterInstance(config).SingleInstance();
            builder.RegisterType<ServiceMapper>().As<IMapper>().InstancePerLifetimeScope();

            //动态代理注入
            assemblyServices.GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IProxyHandle)))
            .ForEach(item => builder.RegisterType(item).Named<IProxyHandle>(item.Name).InstancePerLifetimeScope());
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
            if (_environment.IsDevelopment() || _configuration.Get<AppConfig>().Swagger)
            {
                app.UseCustomSwagger();//文档中间件
            }
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
