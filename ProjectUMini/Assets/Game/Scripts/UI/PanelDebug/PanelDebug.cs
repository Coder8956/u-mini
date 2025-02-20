using Game.Scripts.Common;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.PanelDebug
{
    [UMUIPanelConfig("UI/PanelMain/PanelDebug")]
    public class PanelDebug : UMUIPanel
    {
        [SerializeField] private Button m_btnOpenPanelMain = null;
        [SerializeField] private Button m_btnDumpCreatedUI = null;
        [SerializeField] private Button m_btnPlayBGM_1 = null;
        [SerializeField] private Button m_btnPlayBGM_2 = null;

        protected override void OnCreatePanel()
        {
            // Debug.Log("Create-PanelDebug");
            m_btnOpenPanelMain?.onClick.AddListener(() =>
            {
                // 打开主界面
                UMGR.Get<UMUI>().Open(GameUI.PanelMain, 5);
            });

            m_btnDumpCreatedUI?.onClick.AddListener(() =>
            {
                // 打开主界面
                UMGR.Get<UMUI>().DumpCreatedUI();
            });

            m_btnPlayBGM_1?.onClick.AddListener(() =>
            {
                // 播放 bgm-1
                UMGR.Get<UMAudio>().BGM.Play(GameAudio.BGM_1);
            });

            m_btnPlayBGM_2?.onClick.AddListener(() =>
            {
                // 播放 bgm-2
                UMGR.Get<UMAudio>().BGM.Play(GameAudio.BGM_2);
            });
            // UMGR.Get<UMUI>().Open(pGame);
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