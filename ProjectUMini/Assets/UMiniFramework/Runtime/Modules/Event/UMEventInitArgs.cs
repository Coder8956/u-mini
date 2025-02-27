using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.Event
{
    /// <summary>
    /// 事件模块初始化配置
    /// </summary>
    public class UMEventInitArgs : UMModuleInitArgs
    {
        /// <summary>
        /// 注册的事件标记列表
        /// </summary>
        public List<string> RegisterEventTags { get; set; }
    }
}