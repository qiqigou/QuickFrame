using QuickFrame.Models;
using System.Threading.Tasks;

namespace QuickFrame.IServices
{
    /// <summary>
    /// 授权服务
    /// </summary>
    public interface IAuthService : IService
    {
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TokenOutput> TokenAsync(LoginInput input);
        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<TokenOutput> RefreshTokenAsync(string token);
    }
}
