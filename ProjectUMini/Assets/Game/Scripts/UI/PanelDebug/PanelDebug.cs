using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UnityEngine;

namespace Game.Scripts.UI.PanelDebug
{
    [UMUIPanelConfig("UI/PanelMain/PanelDebug")]
    public class PanelDebug : UMUIPanel
    {
        protected override void OnCreatePanel()
        {
            Debug.Log("Create-PanelDebug");
        }

        protected override void OnDestroyPanel()
        {
        }

        protected override void OnOpenPanel()
        {
            Debug.Log("Open-PanelDebug");
        }

        protected override void OnClosePanel()
        {
        }
    }
}