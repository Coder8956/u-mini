using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.PanelMain
{
    [UMUIPanelConfig("UI/PanelMain/PanelMain")]
    public class PanelMain : UMUIPanel
    {
        [SerializeField] private Button m_btnStartGame;
        [SerializeField] private Button m_btnSet;

        protected override void OnCreatePanel()
        {
            // Debug.Log("Create-PanelMain");
            m_btnStartGame.onClick.AddListener(() =>
            {
                // 进入游戏
            });

            m_btnSet.onClick.AddListener(() =>
            {
                // 打开游戏设置
            });
        }

        protected override void OnDestroyPanel()
        {
            Debug.Log("Destroy-PanelMain");
        }

        protected override void OnOpenPanel()
        {
            Debug.Log("Open-PanelMain");
        }

        protected override void OnClosePanel()
        {
            Debug.Log("Close-PanelMain");
        }
    }
}