using Game.Scripts.Common.GameUI;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
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
                // 打开关卡选择界面
                GameUI.OpenSelectLevel();
            });

            m_btnSet.onClick.AddListener(() =>
            {
                // 打开游戏设置
                GameUI.OpenSet();
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