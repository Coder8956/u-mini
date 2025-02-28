using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.DataPer.UMDataPerHandlers.Interface;

namespace UMiniFramework.Runtime.Modules.DataPer.InitArgs
{
    /// <summary>
    /// 数据持久化 模块配置
    /// </summary>
    public class UMDataPerInitArgs : UMModuleInitArgs
    {
        public UMDataPerInitArgs()
        {
            DataPerHandler = UMDataPerDIArgs.DataPerHandler();
        }

        /// <summary>
        /// 数据持久化处理器类型
        /// </summary>
        public IUMDataPerHandler DataPerHandler { get; set; }
    }
}