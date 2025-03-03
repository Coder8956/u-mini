using UMiniFramework.Runtime.Modules.Base;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.UI.InitArgs
{
    /// <summary>
    /// UI模块 初始化参数
    /// </summary>
    public class UMUIInitArgs : UMModuleInitArgs
    {
        public UMUIInitArgs()
        {
            IsCreateEventSystem = UMUIDIArgs.IsCreateEventSystem();
            UILayerCount = UMUIDIArgs.UILayerCount();
        }

        public bool IsCreateEventSystem { get; set; }

        public int UILayerCount = 0;
    }
}