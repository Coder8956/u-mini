using UMiniFramework.Runtime.Modules.Resource.UMResHandlers;
using UMiniFramework.Runtime.Modules.Resource.UMResHandlers.Interface;

namespace UMiniFramework.Runtime.Modules.Resource.InitArgs
{
    /// <summary>
    /// 资源模块 默认初始化参数
    /// </summary>
    public class UMResDIArgs
    {
        public static IUMResHandler ResHandler()
        {
            return new UMResDefaultHandler();
        }
    }
}