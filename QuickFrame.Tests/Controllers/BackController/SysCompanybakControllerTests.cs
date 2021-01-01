using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QuickFrame.Common;
using QuickFrame.Model;
using QuickFrame.Service;
using System.Net;
using System.Threading.Tasks;

namespace QuickFrame.Controllers.Tests
{
    [TestClass()]
    public class SysCompanybakControllerTests : BaseControllerTest
    {
        public SysCompanybakControllerTests()
        {
            LoginAsync().Wait();
        }

        [TestMethod()]
        public async Task UpdateAsyncTest()
        {
            //创建
            var api = BackUrl(nameof(SysCompanybakController), nameof(SysCompanybakController.CreateAsync));
            var input = new SysCompanybakInput
            {
                ScbCorder = 992,
                ScyCcompanyid = "xxxeee",
                ScbCtype = "备份名xxxx",
                ScsCshopid = "ooooo"
            };
            var res = await _client.PostAsync(api, CreateHttpContent(input));
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            var json = await res.Content.ReadAsStringAsync();
            var data = JSONObject(json);
            var part0 = data["keyvalue"]?["part0"]?.Value<int>();
            var part1 = data["keyvalue"]?["part1"]?.Value<string>();
            Assert.IsNotNull(part0);
            Assert.IsNotNull(part1);
            //查询
            api = BackUrl(nameof(SysCompanybakController), nameof(SysCompanybakController.SingleAsync), part0!, part1!);
            res = await _client.GetAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            json = await res.Content.ReadAsStringAsync();
            var output = JSONDeserialize<v_syscompanybak>(json);
            Assert.IsNotNull(output);
            //修改
            api = BackUrl(nameof(SysCompanybakController), nameof(SysCompanybakController.UpdateAsync), part0, part1);
            var updinput = new SysCompanybakUpdInput
            {
                Timestamp = output.timestamp,
                ScbCtype = "eeeeeeee",
                ScsCshopid = "yyyyyyyy"
            };
            res = await _client.PutAsync(api, CreateHttpContent(updinput));
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            //验证修改
            api = BackUrl(nameof(SysCompanybakController), nameof(SysCompanybakController.SingleAsync), part0, part1);
            res = await _client.GetAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            json = await res.Content.ReadAsStringAsync();
            output = JSONDeserialize<v_syscompanybak>(json);
            Assert.IsNotNull(output);
            Assert.AreEqual(output.scb_ctype, updinput.ScbCtype);
            Assert.AreEqual(output.scs_cshopid, updinput.ScsCshopid);
            //删除
            api = BackUrl(nameof(SysCompanybakController), nameof(SysCompanybakController.DeleteAsync), part0, part1);
            res = await _client.DeleteAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task ObjQueryAsyncTest()
        {
            var api = BackUrl(nameof(SysCompanybakController), nameof(SysCompanybakController.ObjQueryAsync));
            var res = await _client.PostAsync(api, CreateHttpContent(new ObjFilterInput()));
            if (res.StatusCode is not (HttpStatusCode.OK or HttpStatusCode.NoContent))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task SQLQueryAsyncTest()
        {
            var api = BackUrl(nameof(SysCompanybakController), nameof(SysCompanybakController.SQLQueryAsync));
            var res = await _client.PostAsync(api, CreateHttpContent(new SQLFilterInput()));
            if (res.StatusCode is not (HttpStatusCode.OK or HttpStatusCode.NoContent))
            {
                Assert.Fail();
            }
        }
    }
}