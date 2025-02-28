using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.Config.Base;
using UMiniFramework.Runtime.Modules.Config.UMLoadConfigHandlers;
using UMiniFramework.Runtime.Modules.Config.UMLoadConfigHandlers.Interface;

namespace UMiniFramework.Runtime.Modules.Config.InitArgs
{
    /// <summary>
    /// 配置模块 默认初始化参数
    /// </summary>
    public class UMConfigDIArgs
    {
        public static ILoadConfigHandler LoadConfigHandler()
        {
            return new UMResLoadConfigHandler();
        }

        public static List<UMConfigTable> UMConfigTableList()
        {
            return new List<UMConfigTable>();
        }
    }
}