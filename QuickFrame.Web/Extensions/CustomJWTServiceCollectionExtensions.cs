using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using QuickFrame.Common;
using QuickFrame.Web;
using System;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CustomJWTServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomJWTAuthentication(this IServiceCollection services, JwtConfig jwtConfig)
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
            return services;
        }
    }
}
