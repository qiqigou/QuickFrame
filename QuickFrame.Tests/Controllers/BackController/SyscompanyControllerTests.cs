using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QuickFrame.Common;
using QuickFrame.Models;

namespace QuickFrame.Controllers.Tests
{
    [TestClass()]
    public class SyscompanyControllerTests : BaseControllerTest
    {
        public SyscompanyControllerTests()
        {
            LoginAsync().Wait();
        }

        [TestMethod()]
        public async Task UpdateAsyncTest()
        {
            //创建
            var api = BackUrl(nameof(SyscompanyController), nameof(SyscompanyController.CreateAsync));
            var input = new SysCompanyInput
            {
                ScyCcompanyid = "3953",
                ScyCadr = "esdf",
                ScyCcompanyname = "士大夫撒地方",
                ScyCemail = "efsdf",
                ScyCfax = "efwefsdfs",
                ScyCman = "fesdfs",
                ScyCstate = "0",
                ScyCweb = "efsdf",
                ScyCtel = "fesdf"
            };
            var res = await _client.PostAsync(api, CreateHttpContent(input));
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            var json = await res.Content.ReadAsStringAsync();
            var data = JSONObject(json);
            var keyvalue = data["keyvalue"]?.Value<string>();
            Assert.IsNotNull(keyvalue);
            //查询
            api = BackUrl(nameof(SyscompanyController), nameof(SyscompanyController.SingleAsync), keyvalue!);
            res = await _client.GetAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            json = await res.Content.ReadAsStringAsync();
            var output = JSONDeserialize<v_syscompany>(json);
            Assert.IsNotNull(output);
            //修改
            api = BackUrl(nameof(SyscompanyController), nameof(SyscompanyController.UpdateAsync), keyvalue, output.timestamp.ToBase64());
            var updinput = new SysCompanyUpdInput
            {
                ScyCadr = "pppppppppp",
                ScyCcompanyname = "xxxxxxx"
            };
            res = await _client.PutAsync(api, CreateHttpContent(updinput));
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            //验证修改
            api = BackUrl(nameof(SyscompanyController), nameof(SyscompanyController.SingleAsync), keyvalue);
            res = await _client.GetAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            json = await res.Content.ReadAsStringAsync();
            output = JSONDeserialize<v_syscompany>(json);
            Assert.IsNotNull(output);
            Assert.AreEqual(output.scy_cadr, updinput.ScyCadr);
            Assert.AreEqual(output.scy_ccompanyname, updinput.ScyCcompanyname);
            Assert.AreEqual(output.scy_cemail, input.ScyCemail);//验证输入空值是否会被修改
            Assert.AreEqual(output.scy_cfax, input.ScyCfax);
            //删除
            api = BackUrl(nameof(SyscompanyController), nameof(SyscompanyController.DeleteAsync), keyvalue, output.timestamp.ToBase64());
            res = await _client.DeleteAsync(api);
            if (!res.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task ObjQueryAsyncTest()
        {
            var input = new ObjFilterInput
            {
                Condition = new GroupInput
                {
                    Logic = ConstantOptions.LogicConstant.And,
                    Items = new ItemInput[]
                    {
                        new ItemInput
                        {
                            Field = nameof(v_syscompany.scy_ccompanyid),
                            Value = "112",
                            Compare = ConstantOptions.CompareConstant.Contains
                        },
                        new ItemInput
                        {
                            Field = nameof(v_syscompany.scy_ccompanyname),
                            Value = "admin",
                            Compare = ConstantOptions.CompareConstant.Contains
                        }
                    }
                },
                Page = new PageInput
                {
                    Index = 1,
                    Size = 20,
                    Sort = new SortInput[]
                    {
                        new SortInput
                        {
                            OrderBy = nameof(v_syscompany.scy_ccompanyid),
                            Desc = false
                        },
                        new SortInput
                        {
                            OrderBy = nameof(v_syscompany.scy_ccompanyname),
                            Desc = true
                        }
                    }
                }
            };
            var api = BackUrl(nameof(SyscompanyController), nameof(SyscompanyController.ObjQueryAsync));
            var res = await _client.PostAsync(api, CreateHttpContent(input));
            if (res.StatusCode is not (HttpStatusCode.OK or HttpStatusCode.NoContent))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task SQLQueryAsyncTest()
        {
            var api = BackUrl(nameof(SyscompanyController), nameof(SyscompanyController.SQLQueryAsync));
            var res = await _client.PostAsync(api, CreateHttpContent(new SQLFilterInput()));
            if (res.StatusCode is not (HttpStatusCode.OK or HttpStatusCode.NoContent))
            {
                Assert.Fail();
            }
        }
    }
}