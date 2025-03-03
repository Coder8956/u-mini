using Game.Scripts.Common;
using UMiniFramework.Runtime.Modules.UI.Base;
using UMiniFramework.Runtime.Modules.UI.AttributeUMUI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [UMUIPanelATB("UI/PanelMain/PanelMain")]
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
        }

        protected override void OnOpenPanel()
        {
        }

        protected override void OnClosePanel()
        {
        }
    }
}