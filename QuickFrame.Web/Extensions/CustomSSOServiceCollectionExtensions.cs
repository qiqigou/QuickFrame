using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using QuickFrame.Common;
using QuickFrame.Web;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CustomSSOServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomSSOAuthentication(this IServiceCollection services, AppConfig appConfig)
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
            return services;
        }
    }
}
