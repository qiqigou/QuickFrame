using System.Threading.Tasks;
using QuickFrame.Common;
using QuickFrame.Models;

namespace QuickFrame.Services
{
    /// <summary>
    /// 用户审批
    /// </summary>
    [TransientInjection]
    public class UserInfoApprove : ApproveHandleBase<userinfo_us, int>
    {
        protected override Task ApproveAsync(int key)
        {
            throw new System.NotImplementedException();
        }

        protected override Task UnApproveAsync(int key)
        {
            throw new System.NotImplementedException();
        }

        protected override Task HandleAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
