using Microsoft.OpenApi.Models;
using QuickFrame.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CustomSwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, AppConfig appConfig)
        {
            return services.AddSwaggerGen(options =>
            {
                foreach (var item in ConstantOptions.ModulesConstant.Modules)
                {
                    options.SwaggerDoc(item, new OpenApiInfo
                    {
                        Version = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
                        Title = AssemblyOption.RootName
                    });
                }
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{AssemblyOption.CommonName}.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{AssemblyOption.ModelName}.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{AssemblyOption.ServiceName}.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{AssemblyOption.ControllersName}.xml"), true);
                //添加Token按钮
                if (appConfig.IdentityServer.Enable)
                {
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "oauth2",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Description = "OAuth2登录授权",
                        In = ParameterLocation.Header,
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{appConfig.IdentityServer.Url}/connect/authorize"),
                                Scopes =
                                {
                                    { appConfig.SwaggerScope,appConfig.SwaggerScopeName }
                                }
                            }
                        }
                    });
                }
                else
                {
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Value: Bearer {token}",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                }
            });
        }
    }
}
