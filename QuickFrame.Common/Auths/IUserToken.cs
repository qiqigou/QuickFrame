using System.Security.Claims;

namespace QuickFrame.Common
{
    public interface IUserToken
    {
        string Create(Claim[] claims);

        Claim[] Decode(string jwtToken);
    }
}
