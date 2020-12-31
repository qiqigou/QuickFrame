﻿using QuickFrame.Repository;

namespace QuickFrame.Service
{
    public class SysCustomerDeployService : ISysCustomerDeployService
    {
        private readonly ISQLScriptRepository _scriptZxsc;

        public SysCustomerDeployService(ISQLScriptRepository repository)
        {
            _scriptZxsc = repository;
        }
        /// <summary>
        /// 获取账套创建脚本
        /// </summary>
        /// <returns></returns>
        public ScriptOutput GetDbScript()
        {
            return new ScriptOutput { Script = _scriptZxsc.GenerateCreateScript() };
        }
    }
}
