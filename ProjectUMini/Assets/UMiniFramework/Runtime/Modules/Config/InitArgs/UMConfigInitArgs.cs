using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Config.Base;
using UMiniFramework.Runtime.Modules.Config.UMLoadConfigHandlers.Interface;

namespace UMiniFramework.Runtime.Modules.Config.InitArgs
{
    /// <summary>
    /// 配置模块 初始化参数
    /// </summary>
    public class UMConfigInitArgs : UMModuleInitArgs
    {
        public UMConfigInitArgs()
        {
            LoadConfigHandler = UMConfigDIArgs.LoadConfigHandler();
            ConfigTables = UMConfigDIArgs.UMConfigTableList();
        }

        /// <summary>
        /// 配置加载处理器类型
        /// </summary>
        public IUMLoadConfigHandler LoadConfigHandler { get; set; }

        /// <summary>
        /// 需要读取的配置表
        /// </summary>
        public List<UMConfigTable> ConfigTables { get; }
    }
}