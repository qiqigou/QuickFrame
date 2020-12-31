using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QuickFrame.Common;
using QuickFrame.Model;
using QuickFrame.Service;

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
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<MsgOutput> DeleteAsync([FromRoute] string id)
        {
            await _sysCompanyService.DeleteAsync(new[] { id });
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
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<MsgOutput> UpdateAsync([FromRoute] string id, [FromBody] SysCompanyUpdInput input)
        {
            await _sysCompanyService.UpdateAsync(id, input);
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
