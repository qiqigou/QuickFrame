using QuickFrame.Common;

namespace QuickFrame.Service
{
    public class TestProxyHandle : IProxyHandle
    {
        public void InterceptAction()
        {
            "执行Test".WriteInfoLine();
        }
    }
}
