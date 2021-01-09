using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using QuickFrame.Common;
using QuickFrame.Extensions;
using System;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 认证配置
    /// </summary>
    public static class AuthenticationSetup
    {
        /// <summary>
        /// 注入认证配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appconfig"></param>
        /// <param name="jwtconfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationSetup(this IServiceCollection services, AppConfig appconfig, JwtConfig jwtconfig)
        {
            if (appconfig.IdentityServer.Enable)
            {
                AddSSOSetup(services, appconfig);
            }
            else
            {
                AddJWTSetup(services, jwtconfig);
            }
            return services;
        }
        /// <summary>
        /// SSO配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appConfig"></param>
        private static void AddSSOSetup(IServiceCollection services, AppConfig appConfig)
        {
            services.AddAuthentication(options =>
            {
                /*设置相关处理的默认方案*/
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;//默认方案(包含了以下所有配置，但是可以被以下配置覆盖)
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;//认证方案
                options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler);//401消息处理方案
                options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);//403消息处理方案
            })
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = appConfig.IdentityServer.Url;//授权端点
                options.RequireHttpsMetadata = appConfig.IdentityServer.RequireHttps;//需要HTTPS
                options.SupportedTokens = SupportedTokens.Jwt;//Token类型
                options.ApiName = appConfig.IdentityServer.ApiName;//api资源名称
                options.ApiSecret = appConfig.IdentityServer.ApiSecret;//api资源机密串
                options.NameClaimType = JwtClaimTypes.Name;//指定映射到用户名的Claim
                options.RoleClaimType = JwtClaimTypes.Role;//指定映射到角色的Claim
                options.JwtValidationClockSkew = TimeSpan.Zero;//设置Token有效期验证的时间偏移量为0
            })
            .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { });
            //设置授权配置
            services.AddAuthorization(options =>
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new PermissionRequirement(ConstantOptions.RoleConstant.AllRoles));
                options.DefaultPolicy = policy.Build();
            });
        }
        /// <summary>
        /// JWT配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jwtConfig"></param>
        private static void AddJWTSetup(IServiceCollection services, JwtConfig jwtConfig)
        {
            services.AddAuthentication(options =>
            {
                /*设置相关处理的默认方案*/
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;//默认方案(包含了以下所有配置，但是可以被以下配置覆盖)
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//认证方案
                options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler);//401消息处理方案
                options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);//403消息处理方案
            })
            .AddJwtBearer(options =>
            {
                //配置哪些参数需要验证
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//验证签发者
                    ValidateAudience = true,//验证订阅者
                    ValidateLifetime = true,//验证Token生存期
                    ValidateIssuerSigningKey = true,//验证签发者key
                    ValidIssuer = jwtConfig.Issuer,//发布者
                    ValidAudience = jwtConfig.Audience,//订阅者
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey)),
                    ClockSkew = TimeSpan.Zero,//设置时间偏移为0
                    NameClaimType = JwtClaimTypes.Name,//用户名映射
                    RoleClaimType = JwtClaimTypes.Role,//角色映射
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), _ => { });//添加处理方案
            //设置授权配置
            services.AddAuthorization(options =>
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new PermissionRequirement(ConstantOptions.RoleConstant.AllRoles));
                options.DefaultPolicy = policy.Build();
            });
        }
    }
}
