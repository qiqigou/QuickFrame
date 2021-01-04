using Microsoft.AspNetCore.Mvc;
using QuickFrame.Common;
using QuickFrame.Models;
using QuickFrame.Services;
using System.Threading.Tasks;

namespace QuickFrame.Controllers
{
    /// <summary>
    /// 商场
    /// </summary>
    [Area(ConstantOptions.ModulesConstant.Back)]
    [ApiGroup(ConstantOptions.ModulesConstant.Back)]
    public class SysCompanybakController : BaseController
    {
        private readonly ISyscompanybakService _syscompanybakService;

        public SysCompanybakController(ISyscompanybakService syscompanybakService)
        {
            _syscompanybakService = syscompanybakService;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input">输入模型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<KeyOutput<(int, string)>> CreateAsync([FromBody] SysCompanybakInput input)
        {
            var keyvalue = await _syscompanybakService.CreateAsync(input);
            return new KeyOutput<(int, string)>(keyvalue);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="part0">商场ID</param>
        /// <param name="part1">公司编号</param>
        /// <returns></returns>
        [HttpDelete("{part0:int}/{part1}")]
        public async Task<MsgOutput> DeleteAsync([FromRoute] int part0, [FromRoute] string part1)
        {
            await _syscompanybakService.DeleteAsync(new[] { (part0, part1) });
            return MsgOutputOption.OkMsg;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="part0">商场ID</param>
        /// <param name="part1">公司编号</param>
        /// <returns></returns>
        [HttpGet("{part0:int}/{part1}")]
        public Task<v_syscompanybak?> SingleAsync([FromRoute] int part0, [FromRoute] string part1)
        {
            return _syscompanybakService.SingleAsync((part0, part1));
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="part0">商场ID</param>
        /// <param name="part1">公司编号</param>
        /// <param name="input">输入模型</param>
        /// <returns></returns>
        [HttpPut("{part0:int}/{part1}")]
        public async Task<MsgOutput> UpdateAsync([FromRoute] int part0, [FromRoute] string part1, [FromBody] SysCompanybakUpdInput input)
        {
            await _syscompanybakService.UpdateAsync((part0, part1), input);
            return MsgOutputOption.OkMsg;
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="input">查询条件模型</param>
        /// <returns></returns>
        [HttpPost]
        public Task<PageOutput<v_syscompanybak>> ObjQueryAsync([FromBody] ObjFilterInput input)
        {
            return _syscompanybakService.ObjQueryAsync(input);
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="input">查询条件模型</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(PageOutput<v_syscompanybak>), 200)]
        public Task<PageOutput<v_syscompanybak>> SQLQueryAsync([FromBody] SQLFilterInput input)
        {
            return _syscompanybakService.SQLQueryAsync(input);
        }
    }
}
