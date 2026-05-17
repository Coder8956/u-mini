using Game.Scripts.Common;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI.Base;
using UMiniFramework.Runtime.Modules.UI.AttributeUMUI;
using UMiniFramework.Runtime.Modules.UMDataPer;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [UMUIPanelATB("UI/PanelSet/PanelSet")]
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

            m_togAudioBGM.onValueChanged.AddListener((val) => { UMF.Get<UMAudio>().BGM.Mute = val; });
            m_sldAudioBGM.onValueChanged.AddListener((val) => { UMF.Get<UMAudio>().BGM.Volume = val; });

            m_togAudioEffect.onValueChanged.AddListener((val) => { UMF.Get<UMAudio>().Effect.Mute = val; });
            m_sldAudioEffect.onValueChanged.AddListener((val) => { UMF.Get<UMAudio>().Effect.Volume = val; });
        }

        protected override void OnDestroyPanel()
        {
        }

        protected override void OnOpenPanel()
        {
            GameUI.SetMaskColor(gameObject);
            m_togAudioBGM.isOn = UMF.Get<UMAudio>().BGM.Mute;
            m_sldAudioBGM.value = UMF.Get<UMAudio>().BGM.Volume;

            m_togAudioEffect.isOn = UMF.Get<UMAudio>().Effect.Mute;
            m_sldAudioEffect.value = UMF.Get<UMAudio>().Effect.Volume;
        }

        protected override void OnClosePanel()
        {
            // bool audioBGMMute = UMGR.Get<UMAudio>().BGM.Mute;
            // float audioBGMVolume = UMGR.Get<UMAudio>().BGM.Volume;
            // UMGR.Get<UMDataPer>().Save(GameDataPerKey.AudioBGMMute, audioBGMMute.ToString());
            // UMGR.Get<UMDataPer>().Save(GameDataPerKey.AudioBGMVolume, audioBGMVolume.ToString());
            //
            // bool audioEffectMute = UMGR.Get<UMAudio>().Effect.Mute;
            // float audioEffectVolume = UMGR.Get<UMAudio>().Effect.Volume;
            // UMGR.Get<UMDataPer>().Save(GameDataPerKey.AudioEffectMute, audioEffectMute.ToString());
            // UMGR.Get<UMDataPer>().Save(GameDataPerKey.AudioEffectVolume, audioEffectVolume.ToString());
        }
    }
}