using IdentityModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace QuickFrame.Common
{
    [SingletonInjection]
    public class UserToken : IUserToken
    {
        private readonly JwtConfig _jwtConfig;
        public UserToken(IOptions<JwtConfig> options)
        {
            _jwtConfig = options.Value;
        }

        public string Create(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecurityKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var timestamp = DateTime.Now.AddMinutes(_jwtConfig.Expires + _jwtConfig.RefreshExpires).ToTimestamp().ToString();
            claims = claims.Append(new Claim(UserOptions.RefreshToken, timestamp)).ToArray();
            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_jwtConfig.Expires),
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Claim[] Decode(string jwtToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken? securityToken;
            try
            {
                jwtSecurityTokenHandler.ValidateToken(jwtToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,//验证签发者
                    ValidateAudience = true,//验证订阅者
                    ValidateIssuerSigningKey = true,//验证签发者key
                    ValidIssuer = _jwtConfig.Issuer,//发布者
                    ValidAudience = _jwtConfig.Audience,//订阅者
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecurityKey)),//签名秘钥
                    NameClaimType = JwtClaimTypes.Name,//用户名映射
                    RoleClaimType = JwtClaimTypes.Role,//角色映射
                }, out securityToken);
            }
            catch (Exception)
            {
                throw new HandelException(MessageCodeOption.Bad_TokenInvalid);
            }
            if (securityToken is JwtSecurityToken jwtSecurityToken)
            {
                return jwtSecurityToken?.Claims?.ToArray() ?? Array.Empty<Claim>();
            }
            else
            {
                throw new HandelException(MessageCodeOption.Bad_TokenInvalid);
            }
        }
    }
}
