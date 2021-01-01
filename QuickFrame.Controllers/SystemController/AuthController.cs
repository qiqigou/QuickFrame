using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Common;
using QuickFrame.Service;
using System.Threading.Tasks;

namespace QuickFrame.Controllers
{
    /// <summary>
    /// 授权端点
    /// </summary>
    [Area(ConstantOptions.ModulesConstant.System)]
    [ApiGroup(ConstantOptions.ModulesConstant.System)]
    public class AuthController : BaseController
    {
        private readonly IAuthService _tokenService;

        public AuthController(IAuthService tokenService)
        {
            _tokenService = tokenService;
        }
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<TokenOutput> TokenAsync([FromQuery] LoginInput input)
        {
            return await _tokenService.TokenAsync(input);
        }
        /// <summary>
        /// 刷新Token(以旧换新)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("{token}")]
        [AllowAnonymous]
        public async Task<TokenOutput> RefreshTokenAsync([FromRoute] string token)
        {
            return await _tokenService.RefreshTokenAsync(token);
        }
    }
}
