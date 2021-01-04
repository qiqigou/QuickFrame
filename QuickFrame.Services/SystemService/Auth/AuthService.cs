using QuickFrame.Common;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuickFrame.Services
{
    /// <summary>
    /// 授权服务
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserToken _userToken;

        public AuthService(IUserToken userToken)
        {
            _userToken = userToken;
        }
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<TokenOutput> TokenAsync(LoginInput input)
        {
            Claim[] claims =
            {
                new Claim(UserOptions.UserId,"001"),
                new Claim(UserOptions.Role,"admin"),
                new Claim(UserOptions.DBName,input.DbName),
                new Claim(UserOptions.UserName,input.UserName)
            };
            return Task.FromResult(new TokenOutput { Token = _userToken.Create(claims) });
        }
        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<TokenOutput> RefreshTokenAsync(string token)
        {
            var userClaims = _userToken.Decode(token);
            var refexp = userClaims.FirstOrDefault(x => x.Type == UserOptions.RefreshToken)?.Value ?? throw new HandelException(MessageCodeOption.Bad_Format, "Token");
            if (long.TryParse(refexp, out var refexplong))
            {
                _ = refexplong > DateTime.Now.ToTimestamp() ? true : throw new HandelException(MessageCodeOption.Bad_Token);
                return TokenAsync(new LoginInput
                {
                    UserName = userClaims.FirstOrDefault(x => x.Type == UserOptions.UserName)?.Value ?? string.Empty,
                    DbName = userClaims.FirstOrDefault(x => x.Type == UserOptions.DBName)?.Value ?? string.Empty,
                });
            }
            throw new HandelException(MessageCodeOption.Bad_Format, $"Token中{UserOptions.RefreshToken}");
        }
    }
}
