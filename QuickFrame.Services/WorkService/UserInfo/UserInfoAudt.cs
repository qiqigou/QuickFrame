using System;
using System.Threading.Tasks;
using QuickFrame.Common;
using QuickFrame.Models;

namespace QuickFrame.Services
{
    /// <summary>
    /// 用户审核
    /// </summary>
    [TransientInjection]
    public class UserInfoAudt : AudtHandleBase<userinfo_us, int>
    {
        protected override Task AudtAsync(int key)
        {
            throw new NotImplementedException();
        }

        protected override Task UnAudtAsync(int key)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
