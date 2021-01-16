using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuickFrame.Controllers.Tests
{
    [TestClass()]
    public class SysCustomerDeployControllerTests : BaseControllerTest
    {
        public SysCustomerDeployControllerTests()
        {
            LoginAsync().Wait();
        }

        [TestMethod()]
        public async Task GetDbScriptAsyncTest()
        {
            var api = BackUrl(nameof(SysCustomerDeployController), nameof(SysCustomerDeployController.GetDbScriptAsync));
            var res = await _client.GetAsync(api);
            if (!res.IsSuccessStatusCode) Assert.Fail();
        }
    }
}