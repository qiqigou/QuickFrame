using System.Threading.Tasks;

namespace QuickFrame.Service
{
    public interface IAuthService: IService
    {
        Task<TokenOutput> TokenAsync(LoginInput input);

        Task<TokenOutput> RefreshTokenAsync(string token);
    }
}
