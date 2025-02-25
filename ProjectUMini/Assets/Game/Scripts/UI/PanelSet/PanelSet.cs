using Game.Scripts.Common.GameUI;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [UMUIPanelConfig("UI/PanelSet/PanelSet")]
    public class PanelSet : UMUIPanel
    {
        [SerializeField] private Button m_btnClose;

        [SerializeField] private Toggle m_togAudioBGM;
        [SerializeField] private Slider m_sldAudioBGM;

        [SerializeField] private Toggle m_togAudioEffect;
        [SerializeField] private Slider m_sldAudioEffect;

        protected override void OnCreatePanel()
        {
            m_btnClose.onClick.AddListener(() => { GameUI.CloseSet(); });
        }

        protected override void OnDestroyPanel()
        {
        }

        protected override void OnOpenPanel()
        {
            GameUI.SetMaskColor(gameObject);
            m_togAudioBGM.isOn = UMGR.Get<UMAudio>().BGM.Mute;
            m_sldAudioBGM.value = UMGR.Get<UMAudio>().BGM.Volume;

            m_togAudioEffect.isOn = UMGR.Get<UMAudio>().Effect.Mute;
            m_sldAudioEffect.value = UMGR.Get<UMAudio>().Effect.Volume;
        }

        protected override void OnClosePanel()
        {
        }
    }
}