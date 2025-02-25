using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.DataPer.Interface;

namespace UMiniFramework.Runtime.Modules.DataPer
{
    /// <summary>
    /// UM 模块配置
    /// </summary>
    public class UMDataPerConfig : UMModuleConfig
    {
        /// <summary>
        /// 数据持久化处理器类型
        /// </summary>
        public IUMDataPerHandler DataPerHandler { get; set; }
    }
}