using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Mime;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using QuickFrame.Common;

namespace QuickFrame.Web
{
    /// <summary>
    /// 认证结果处理
    /// </summary>
    public class ResponseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly JsonOptions _jsonOptions;

        public ResponseAuthenticationHandler(
            IOptions<JsonOptions> jsonOptions,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        ) : base(options, logger, encoder, clock)
        {
            _jsonOptions = jsonOptions.Value;
        }
        /// <summary>
        /// 未使用，不需要实现
        /// </summary>
        /// <returns></returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync() => throw new NotImplementedException();
        /// <summary>
        /// 未认证
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = MediaTypeNames.Application.Json;
            Response.StatusCode = MessageCodeOption.Unauthorized.Code;
            return Response.WriteAsync(JsonSerializer.Serialize(new MsgOutput(MessageCodeOption.Unauthorized), _jsonOptions.JsonSerializerOptions));
        }
        /// <summary>
        /// 未授权
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = MediaTypeNames.Application.Json;
            Response.StatusCode = MessageCodeOption.Forbidden.Code;
            return Response.WriteAsync(JsonSerializer.Serialize(new MsgOutput(MessageCodeOption.Forbidden), _jsonOptions.JsonSerializerOptions));
        }
    }

}
