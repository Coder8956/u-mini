using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.DataPer.Interface;
using UMiniFramework.Runtime.Modules.Resource.Interface;

namespace UMiniFramework.Runtime.Modules.Resource
{
    /// <summary>
    /// 资源模块初始化配置
    /// </summary>
    public class UMResInitArgs : UMModuleInitArgs
    {
        /// <summary>
        /// 资源加载处理器类型
        /// </summary>
        public IUMResHandler ResHandler { get; set; }
    }
}