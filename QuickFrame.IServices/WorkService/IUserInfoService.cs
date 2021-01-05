using QuickFrame.Models;

namespace QuickFrame.IServices
{
    public interface IUserInfoService : IService, IHandle<UserInfoInput, UserInfoUpdInput, int>, IQuery<v_userinfo, int>, IAudt<userinfo_us, int>, IApprove<userinfo_us, int>
    {

    }
}
