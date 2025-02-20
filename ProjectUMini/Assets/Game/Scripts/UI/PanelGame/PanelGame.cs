using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UnityEngine;

namespace Game.Scripts.UI.PanelGame
{
    [UMUIPanelConfig("UI/PanelMain/PanelGame")]
    public class PanelGame : UMUIPanel
    {
        protected override void OnCreatePanel()
        {
            Debug.Log("Create-PanelGame");
        }

        protected override void OnDestroyPanel()
        {
        }

        protected override void OnOpenPanel()
        {
            Debug.Log("Open-PanelGame");
        }

        protected override void OnClosePanel()
        {
        }
    }
}