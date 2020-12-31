using QuickFrame.Common;

namespace QuickFrame.Service
{
    public class Test1ProxyHandle : IProxyHandle
    {
        public void InterceptAction()
        {
            "执行Test1".WriteInfoLine();
        }
    }
}
