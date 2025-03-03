using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.Event.InitArgs
{
    /// <summary>
    /// 事件模块 初始化参数
    /// </summary>
    public class UMEventInitArgs : UMModuleInitArgs
    {
        public UMEventInitArgs()
        {
            RegisterEventTags = UMEventDIArgs.RegisterEventTagList();
        }

        /// <summary>
        /// 注册的事件标记列表
        /// </summary>
        public List<string> RegisterEventTags { get; set; }
    }
}