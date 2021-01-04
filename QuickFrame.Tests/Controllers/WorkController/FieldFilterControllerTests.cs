using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QuickFrame.Common;
using QuickFrame.Models;
using QuickFrame.Services;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuickFrame.Controllers.Tests
{
    [TestClass()]
    public class FieldFilterControllerTests : BaseControllerTest
    {
        public FieldFilterControllerTests()
        {
            LoginAsync().Wait();
        }

        [TestMethod()]
        public async Task UpdateMainChildAsyncTest()
        {
            //创建
            var api = WorkUrl(nameof(FieldFilterController), nameof(FieldFilterController.CreateMainChildAsync));
            var input = new MainChildInput<FieldFilterInput, FieldFiltercInput>
            {
                Main = new FieldFilterInput
                {
                    FgSource = typeof(userinfo_us).FullName ?? nameof(userinfo_us),
                    FgDest = $"{typeof(userinfo_us).FullName ?? nameof(userinfo_us)}_DTO"
                },
                Child = new[]
                {
                    new FieldFiltercInput{ FgcField = nameof(userinfo_us.name),FgcIorder = 1 },
                    new FieldFiltercInput{ FgcField = nameof(userinfo_us.age),FgcIorder = 2 },
                }
            };
            var res = await _client.PostAsync(api, CreateHttpContent(input));
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            var json = await res.Content.ReadAsStringAsync();
            var data = JSONObject(json);
            var keyvalue = data["keyvalue"]?.Value<int>();
            Assert.IsNotNull(keyvalue);
            //查询
            api = WorkUrl(nameof(FieldFilterController), nameof(FieldFilterController.SingleMainChildAsync), keyvalue!);
            res = await _client.GetAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            json = await res.Content.ReadAsStringAsync();
            var output = JSONDeserialize<MainChildOutput<v_fieldfilter, v_fieldfilterc>>(json);
            Assert.IsNotNull(output);
            //修改
            api = WorkUrl(nameof(FieldFilterController), nameof(FieldFilterController.UpdateMainChildAsync), keyvalue);
            var updinput = new MainChildInput<FieldFilterUpdInput, FieldFiltercUpdInput>
            {
                Main = new FieldFilterUpdInput
                {
                    Timestamp = output.Main.timestamp
                },
                Child = new[]
                {
                    new FieldFiltercUpdInput{ FgcField = nameof(userinfo_us.name),FgcIorder = 2 },
                    new FieldFiltercUpdInput{ FgcField = nameof(userinfo_us.age),FgcIorder = 3 },
                    new FieldFiltercUpdInput{ FgcField = nameof(userinfo_us.sex),FgcIorder = 4 },
                    new FieldFiltercUpdInput{ FgcField = nameof(userinfo_us.birthday),FgcIorder = 5 }
                }
            };
            var mainUpdinput =
            res = await _client.PutAsync(api, CreateHttpContent(updinput));
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            //验证修改
            api = WorkUrl(nameof(FieldFilterController), nameof(FieldFilterController.SingleMainChildAsync), keyvalue);
            res = await _client.GetAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            json = await res.Content.ReadAsStringAsync();
            output = JSONDeserialize<MainChildOutput<v_fieldfilter, v_fieldfilterc>>(json);
            Assert.IsNotNull(output);
            Assert.AreEqual(output.Child.Count(), 4);
            Assert.AreEqual(output.Child.Where(x => x.fgc_iorder == 1).Count(), 0);
            //删除
            api = WorkUrl(nameof(FieldFilterController), nameof(FieldFilterController.DeleteMainChildAsync), keyvalue);
            res = await _client.DeleteAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task ObjQueryMainAsyncTest()
        {
            var api = WorkUrl(nameof(FieldFilterController), nameof(FieldFilterController.ObjQueryMainAsync));
            var res = await _client.PostAsync(api, CreateHttpContent(new ObjFilterInput()));
            if (res.StatusCode is not (HttpStatusCode.OK or HttpStatusCode.NoContent))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task SQLQueryMainAsyncTest()
        {
            var api = WorkUrl(nameof(FieldFilterController), nameof(FieldFilterController.SQLQueryMainAsync));
            var res = await _client.PostAsync(api, CreateHttpContent(new SQLFilterInput()));
            if (res.StatusCode is not (HttpStatusCode.OK or HttpStatusCode.NoContent))
            {
                Assert.Fail();
            }
        }
    }
}