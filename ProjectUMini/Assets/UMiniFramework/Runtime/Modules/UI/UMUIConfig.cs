using System.Collections;
using UMiniFramework.Runtime.Modules.Base;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.UI
{
    public class UMUIConfig : UMModuleConfig
    {
        public bool IsCreateEventSystem { get; set; }

        public RenderMode CanvasRenderMode = RenderMode.ScreenSpaceOverlay;
    }
}