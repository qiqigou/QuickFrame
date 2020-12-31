using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace QuickFrame.Web
{
    /// <summary>
    /// 基于权限授权
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 预设角色(无需验证权限的角色)
        /// </summary>
        private readonly string[] _roles;
        /// <summary>
        /// 预设角色(只读)
        /// </summary>
        public IReadOnlyCollection<string> Roles => _roles;

        public PermissionRequirement(string[] roles)
        {
            _roles = roles;
        }
    }
}
