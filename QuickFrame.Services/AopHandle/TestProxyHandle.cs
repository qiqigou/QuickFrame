using QuickFrame.Common;

namespace QuickFrame.Services
{
    public class TestProxyHandle : IProxyHandle
    {
        public void InterceptAction()
        {
            "执行Test".WriteInfoLine();
        }
    }
}
