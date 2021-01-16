using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickFrame.Models;

namespace QuickFrame.Controllers.Tests
{
    [TestClass()]
    public class LayoutControllerTests : BaseControllerTest
    {
        [TestMethod()]
        public async Task QueryViewColumnsAsyncTest()
        {
            var api = SystemUrl(nameof(LayoutController), nameof(LayoutController.QueryViewColumnsAsync), nameof(userinfo_us));
            var res = await _client.GetAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task QueryViewsAsyncTest()
        {
            var api = SystemUrl(nameof(LayoutController), nameof(LayoutController.QueryViewsAsync));
            var res = await _client.GetAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
        }
    }
}