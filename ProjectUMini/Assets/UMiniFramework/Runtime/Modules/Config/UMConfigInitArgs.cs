using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Config.Base;
using UMiniFramework.Runtime.Modules.Config.Interface;

namespace UMiniFramework.Runtime.Modules.Config
{
    /// <summary>
    /// UM 模块配置
    /// </summary>
    public class UMConfigInitArgs : UMModuleInitArgs
    {
        /// <summary>
        /// 配置加载处理器类型
        /// </summary>
        public ILoadConfigHandler LoadConfigHandler { get; set; }

        /// <summary>
        /// 需要读取的配置表
        /// </summary>
        public List<UMConfigTable> ConfigTables { get; set; }
    }
}