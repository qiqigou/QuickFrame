using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Controllers.Tests
{
    [TestClass()]
    public class AuthControllerTests : BaseControllerTest
    {
        [TestMethod()]
        public Task TokenAsyncTest()
        {
            return GetApiKeyAsync(User);
        }

        [TestMethod()]
        public async Task RefreshTokenAsyncTest()
        {
            await LoginAsync();
            var token = _client.DefaultRequestHeaders.Authorization?.Parameter ?? await GetApiKeyAsync(User);
            var api = SystemUrl(nameof(AuthController), nameof(AuthController.RefreshTokenAsync), token);
            var res = await _client.GetAsync(api);
            Assert.IsTrue(res.IsSuccessStatusCode);
            var json = await res.Content.ReadAsStringAsync();
            var data = JSONObject(json);
            token = data?["token"]?.Value<string>();
            Assert.IsNotNull(token);
        }
    }
}