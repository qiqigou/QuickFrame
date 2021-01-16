using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Common;
using QuickFrame.IServices;
using QuickFrame.Models;

namespace QuickFrame.Controllers
{
    /// <summary>
    /// 公司信息
    /// </summary>
    [Area(ConstantOptions.ModulesConstant.Back)]
    [ApiGroup(ConstantOptions.ModulesConstant.Back)]
    public class SyscompanyController : BaseController
    {
        private readonly ISysCompanyService _sysCompanyService;

        public SyscompanyController(ISysCompanyService sysCompanyService)
        {
            _sysCompanyService = sysCompanyService;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<KeyOutput<string>> CreateAsync([FromBody] SysCompanyInput input)
        {
            var keyvalue = await _sysCompanyService.CreateAsync(input);
            return new KeyOutput<string>(keyvalue);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<MsgOutput> DeleteAsync([FromRoute] string id, [FromRoute] byte[] timestamp)
        {
            var key = new KeyStamp<string>(id, timestamp);
            await _sysCompanyService.DeleteAsync(new[] { key });
            return MsgOutputOption.OkMsg;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<v_syscompany?> SingleAsync([FromRoute] string id)
        {
            return _sysCompanyService.SingleAsync(id);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <param name="timestamp"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}/{timestamp}")]
        public async Task<MsgOutput> UpdateAsync([FromRoute] string id, [FromRoute] byte[] timestamp, [FromBody] SysCompanyUpdInput input)
        {
            await _sysCompanyService.UpdateAsync(id, timestamp, input);
            return MsgOutputOption.OkMsg;
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<PageOutput<v_syscompany>> ObjQueryAsync([FromBody] ObjFilterInput input)
        {
            return _sysCompanyService.ObjQueryAsync(input);
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<PageOutput<v_syscompany>> SQLQueryAsync([FromBody] SQLFilterInput input)
        {
            return _sysCompanyService.SQLQueryAsync(input);
        }
    }
}
