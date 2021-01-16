using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Common;
using QuickFrame.IServices;
using QuickFrame.Models;

namespace QuickFrame.Controllers
{
    /// <summary>
    /// 测试端点
    /// </summary>
    [Area(ConstantOptions.ModulesConstant.Work)]
    [ApiGroup(ConstantOptions.ModulesConstant.Work)]
    public class UserInfoController : BaseController
    {
        private readonly IUserInfoService _userinfoService;
        private readonly IFieldFilterProvider _filterProvider;

        public UserInfoController(IUserInfoService userinfoService, IFieldFilterProvider filterProvide)
        {
            _userinfoService = userinfoService;
            _filterProvider = filterProvide;
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<KeyOutput<int>> CreateAsync([FromBody] UserInfoInput input)
        {
            var keyvalue = await _userinfoService.CreateAsync(input);
            return new KeyOutput<int>(keyvalue);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:int}/{timestamp}")]
        public async Task<MsgOutput> DeleteAsync([FromRoute] int id, [FromRoute] byte[] timestamp)
        {
            var key = new KeyStamp<int>(id, timestamp);
            await _userinfoService.DeleteAsync(new[] { key });
            return MsgOutputOption.OkMsg;
        }
        /// <summary>
        /// 获取用户(含字段过滤)
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(v_userinfo), 200)]
        public async Task<object?> SingleAsync([FromRoute] int id)
        {
            var user = await _userinfoService.SingleAsync(id);
            return _filterProvider.Filter(user);
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id:int}/{timestamp}")]
        public async Task<MsgOutput> UpdateAsync([FromRoute] int id, [FromRoute] byte[] timestamp, [FromBody] UserInfoUpdInput input)
        {
            await _userinfoService.UpdateAsync(id, timestamp, input);
            return MsgOutputOption.OkMsg;
        }
        /// <summary>
        /// 条件查询(含字段过滤)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(PageOutput<v_userinfo>), 200)]
        public async Task<PageOutput<object>> ObjQueryAsync([FromBody] ObjFilterInput input)
        {
            var page = await _userinfoService.ObjQueryAsync(input);
            return PageOutput.Convert(page, _filterProvider.FilterRange(page.DataList));
        }
        /// <summary>
        /// 条件查询(含字段过滤)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(PageOutput<v_userinfo>), 200)]
        public async Task<PageOutput<object>> SQLQueryAsync([FromBody] SQLFilterInput input)
        {
            var page = await _userinfoService.SQLQueryAsync(input);
            return PageOutput.Convert(page, _filterProvider.FilterRange(page.DataList));
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}/{timestamp}")]
        public async Task<MsgOutput> AudtAsync([FromRoute] int id)
        {
            await _userinfoService.AudtRangeAsync(new[] { id });
            return MsgOutputOption.OkMsg;
        }
        /// <summary>
        /// 弃审
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}/{timestamp}")]
        public async Task<MsgOutput> UnAudtAsync([FromRoute] int id)
        {
            await _userinfoService.UnAudtRangeAsync(new[] { id });
            return MsgOutputOption.OkMsg;
        }
        /// <summary>
        /// 审批
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}/{timestamp}")]
        public async Task<MsgOutput> ApproveAsync([FromRoute] int id)
        {
            await _userinfoService.ApproveRangeAsync(new[] { id });
            return MsgOutputOption.OkMsg;
        }
        /// <summary>
        /// 弃批
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}/{timestamp}")]
        public async Task<MsgOutput> UnApproveAsync([FromRoute] int id)
        {
            await _userinfoService.UnApproveRangeAsync(new[] { id });
            return MsgOutputOption.OkMsg;
        }
    }
}
