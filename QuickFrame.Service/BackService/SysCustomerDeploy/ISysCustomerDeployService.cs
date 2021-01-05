namespace QuickFrame.Service
{
    public interface ISysCustomerDeployService : IService
    {
        /// <summary>
        /// 获取账套创建脚本
        /// </summary>
        /// <returns></returns>
        ScriptOutput GetDbScript();
    }
}
