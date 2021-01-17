using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickFrame.Common;
using QuickFrame.Models;
using QuickFrame.Repositorys;

namespace QuickFrame.Controllers.Tests
{
    [TestClass()]
    public class UserInfoControllerTests : BaseControllerTest
    {
        public UserInfoControllerTests()
        {
            LoginAsync().Wait();
        }

        [TestMethod()]
        public async Task UpdateAsyncTest()
        {
            //清空
            var repository = _serviceProvider.GetRequiredService<IUserinfoRepository>();
            await repository.ExecuteSqlRawAsync($"delete from {nameof(userinfo_us)} where 1=1");
            //创建
            var api = WorkUrl(nameof(UserInfoController), nameof(UserInfoController.CreateAsync));
            var input = new UserInfoInput
            {
                Name = "wyll",
                Age = 56,
                Sex = "女"
            };
            var res = await _client.PostAsync(api, CreateHttpContent(input));
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            var json = await res.Content.ReadAsStringAsync();
            var keyvalue = JSONDeserialize<KeyOutput<int>>(json).KeyValue;
            //查询
            api = WorkUrl(nameof(UserInfoController), nameof(UserInfoController.SingleAsync), keyvalue);
            res = await _client.GetAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            json = await res.Content.ReadAsStringAsync();
            var output = JSONDeserialize<v_userinfo>(json);
            Assert.IsNotNull(output);
            //修改
            api = WorkUrl(nameof(UserInfoController), nameof(UserInfoController.UpdateAsync), keyvalue, output.timestamp.ToBase64());
            var updinput = new UserInfoUpdInput
            {
                Age = 21,
                Sex = "男"
            };
            res = await _client.PutAsync(api, CreateHttpContent(updinput));
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            //验证修改
            api = WorkUrl(nameof(UserInfoController), nameof(UserInfoController.SingleAsync), keyvalue);
            res = await _client.GetAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            json = await res.Content.ReadAsStringAsync();
            output = JSONDeserialize<v_userinfo>(json);
            Assert.IsNotNull(output);
            Assert.AreEqual(output.age, updinput.Age);
            Assert.AreEqual(output.sex, updinput.Sex);
            //删除
            api = WorkUrl(nameof(UserInfoController), nameof(UserInfoController.DeleteAsync), keyvalue, output.timestamp.ToBase64());
            res = await _client.DeleteAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task ObjQueryAsyncTest()
        {
            var api = WorkUrl(nameof(UserInfoController), nameof(UserInfoController.ObjQueryAsync));
            var res = await _client.PostAsync(api, CreateHttpContent(new ObjFilterInput()));
            if (res.StatusCode is not (HttpStatusCode.OK or HttpStatusCode.NoContent))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task SQLQueryAsyncTest()
        {
            var api = WorkUrl(nameof(UserInfoController), nameof(UserInfoController.SQLQueryAsync));
            var res = await _client.PostAsync(api, CreateHttpContent(new SQLFilterInput()));
            if (res.StatusCode is not (HttpStatusCode.OK or HttpStatusCode.NoContent))
            {
                Assert.Fail();
            }
        }
    }
}