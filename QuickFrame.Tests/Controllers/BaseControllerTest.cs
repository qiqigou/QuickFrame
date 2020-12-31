using IdentityModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using QuickFrame.Common;
using QuickFrame.Service;
using QuickFrame.Tests;

namespace QuickFrame.Controllers.Tests
{
    public class BaseControllerTest : BaseTest
    {
        private static string JoinParams(object[] query) => query.Any() ? "/" + string.Join('/', query) : string.Empty;
        protected string SystemUrl(string controller, string action, params object[] query)
            => $"/api/{ConstantOptions.ModulesConstant.System}/{controller.Replace("Controller", "")}/{action.Replace("Async", "")}{JoinParams(query)}";
        protected string WorkUrl(string controller, string action, params object[] query)
            => $"/api/{ConstantOptions.ModulesConstant.Work}/{controller.Replace("Controller", "")}/{action.Replace("Async", "")}{JoinParams(query)}";
        protected string BackUrl(string controller, string action, params object[] query)
            => $"/api/{ConstantOptions.ModulesConstant.Back}/{controller.Replace("Controller", "")}/{action.Replace("Async", "")}{JoinParams(query)}";

        public static LoginInput User => new LoginInput { UserName = "wyl", PassWord = "123123", DbName = "zxsccore" };

        /// <summary>
        /// 创建请求HttpContext
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteArrayContent CreateHttpContent(object input)
        {
            var content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(input));
            var httpContent = new ByteArrayContent(content);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;charset=UTF-8");
            return httpContent;
        }
        /// <summary>
        /// 序列化json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JSONDeserialize<T>(string json)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 反序列化json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JSONSerialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 转为JObject(含Linq支持,比较方便)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JObject JSONObject(string json)
        {
            return JObject.Parse(json);
        }
        /// <summary>
        /// 转为JArray
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JArray JSONArray(string json)
        {
            return JArray.Parse(json);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public async Task LoginAsync()
        {
            if (_appConfig.IdentityServer.Enable)
            {
                _client.SetBearerToken(await GetOAuthTokenAsync());
            }
            else
            {
                _client.SetBearerToken(await GetApiKeyAsync(User));
            }
        }
        /// <summary>
        /// oauth2.0登录
        /// </summary>
        /// <returns></returns>
        protected async Task<string> GetOAuthTokenAsync()
        {
            var httpClient = new HttpClient();
            var disco = await httpClient.GetDiscoveryDocumentAsync(_appConfig.IdentityServer.Url);
            if (disco.IsError) throw disco.Exception;
            var tokenResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "quickframe_test",
                ClientSecret = "quickframe_test",
                UserName = "admin",
                Password = "123123",
                Scope = "quickframe",
            });
            Assert.IsFalse(tokenResponse.IsError);
            return tokenResponse.AccessToken;
        }
        /// <summary>
        /// apikey登录
        /// </summary>
        /// <returns></returns>
        protected async Task<string> GetApiKeyAsync(LoginInput input)
        {
            var api = SystemUrl(nameof(AuthController), nameof(AuthController.TokenAsync));
            var res = await _client.GetAsync($"{api}?username={input.UserName}&password={input.PassWord}&dbname={input.DbName}");
            if (!res.IsSuccessStatusCode) Assert.Fail();
            var json = await res.Content.ReadAsStringAsync();
            var data = JSONObject(json);
            var token = data?["token"]?.Value<string>();
            Assert.IsNotNull(token);
            return token!;
        }
    }
}
