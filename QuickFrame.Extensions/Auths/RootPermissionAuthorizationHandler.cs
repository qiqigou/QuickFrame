using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace QuickFrame.Extensions
{
    /// <summary>
    /// 预设角色授权策略
    /// </summary>
    public class RootPermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User?.Identity?.IsAuthenticated ?? false)
            {
                if (requirement.Roles.Any(role => context.User.IsInRole(role)))
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
