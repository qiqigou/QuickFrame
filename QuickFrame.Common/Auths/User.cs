using Microsoft.AspNetCore.Http;
using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [SingletonInjection]
    public class User : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public User(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id
        {
            get
            {
                var info = _accessor?.HttpContext?.User?.FindFirst(UserOptions.UserId);
                return info?.Value.NotNull() ?? false ? info.Value : string.Empty;
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name
        {
            get
            {
                var info = _accessor?.HttpContext?.User?.FindFirst(UserOptions.UserName);
                return info?.Value.NotNull() ?? false ? info.Value : string.Empty;
            }
        }
        /// <summary>
        /// 角色
        /// </summary>
        /// <value></value>
        public string Role
        {
            get
            {
                var info = _accessor?.HttpContext?.User?.FindFirst(UserOptions.Role);
                return info?.Value.NotNull() ?? false ? info.Value : string.Empty;
            }
        }
        /// <summary>
        /// 数据库名
        /// </summary>
        public string DBName
        {
            get
            {
                var info = _accessor?.HttpContext?.User?.FindFirst(UserOptions.DBName);
                return info?.Value.NotNull() ?? false ? info.Value : string.Empty;
            }
        }
        /// <summary>
        /// 数据签名
        /// </summary>
        public string Sign
        {
            get
            {
                return _accessor?.HttpContext?.Request?.Headers?[UserOptions.Sign] ?? string.Empty;
            }
        }
    }
}
