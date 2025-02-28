using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Resource.UMResHandlers;
using UMiniFramework.Runtime.Modules.Resource.UMResHandlers.Interface;

namespace UMiniFramework.Runtime.Modules.Resource.InitArgs
{
    /// <summary>
    /// 资源模块 初始化参数
    /// </summary>
    public class UMResInitArgs : UMModuleInitArgs
    {
        public UMResInitArgs()
        {
            ResHandler = new UMResDefaultHandler();
        }

        /// <summary>
        /// 资源加载处理器类型
        /// </summary>
        public IUMResHandler ResHandler { get; set; }
    }
}