using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using QuickFrame.Common;

namespace QuickFrame.Web
{
    /// <summary>
    /// API权限授权策略
    /// </summary>
    public class ApiPermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUser _user;

        public ApiPermissionAuthorizationHandler(IUser user)
        {
            _user = user;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.HasSucceeded) return Task.CompletedTask;
            if (context.HasFailed) return Task.CompletedTask;
            if (context.Resource is DefaultHttpContext request)
            {
                var area = request.Request.RouteValues["area"];
                var controller = request.Request.RouteValues["controller"];
                var action = request.Request.RouteValues["action"];
                if (_user.Id.NotNull())
                {
                    //...这里省略权限验证过程
                    Console.WriteLine($"{area}/{controller}/{action}");
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
