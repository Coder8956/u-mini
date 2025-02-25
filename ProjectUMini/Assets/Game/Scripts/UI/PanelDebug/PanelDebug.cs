using Game.Scripts.Common;
using Game.Scripts.Common.GameUI;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Config;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UMiniFramework.Runtime.Modules.UMDataPer;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [UMUIPanelConfig("UI/PanelDebug/PanelDebug")]
    public class PanelDebug : UMUIPanel
    {
        [SerializeField] private GameObject m_DebugGO;
        [SerializeField] private Button m_btnSwitch = null;

        [SerializeField] private Button m_btnOpenPanelMain = null;
        [SerializeField] private Button m_btnOpenPanelSet = null;
        [SerializeField] private Button m_btnDumpCreatedUI = null;

        [SerializeField] private Button m_btnPlayBGM_1 = null;
        [SerializeField] private Button m_btnPlayBGM_2 = null;

        [SerializeField] private Button m_btnPlayEffect_1 = null;
        [SerializeField] private Button m_btnPlayEffect_2 = null;
        [SerializeField] private Button m_btnPlayEffect_3 = null;
        [SerializeField] private Button m_btnPrintASInfo = null;
        [SerializeField] private Toggle m_togAudioEffect;
        [SerializeField] private Slider m_sldAudioEffect;

        [SerializeField] private Button m_btnDataSave = null;
        [SerializeField] private Button m_btnDataRead = null;
        [SerializeField] private Button m_btnDataDelete = null;
        [SerializeField] private Button m_btnDataDeleteAll = null;

        [SerializeField] private Button m_btnReadConfig = null;

        protected override void OnCreatePanel()
        {
            m_DebugGO.SetActive(false);

            m_btnSwitch?.onClick.AddListener(() =>
            {
                // 打开/关闭 debug 对象
                m_DebugGO.SetActive(!m_DebugGO.activeSelf);
            });

            // Debug.Log("Create-PanelDebug");
            m_btnOpenPanelMain?.onClick.AddListener(() =>
            {
                // 打开主界面
                GameUI.OpenMain();
            });

            m_btnOpenPanelSet?.onClick.AddListener(() =>
            {
                // 打开设置界面
                GameUI.OpenSet();
            });

            m_btnDumpCreatedUI?.onClick.AddListener(() =>
            {
                // 输出创建的 UI
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

            m_btnPlayEffect_1?.onClick.AddListener(() =>
            {
                // 播放 effect-1
                UMGR.Get<UMAudio>().Effect.Play(GameAudio.Effect_1);
            });
            m_btnPlayEffect_2?.onClick.AddListener(() =>
            {
                // 播放 effect-2
                UMGR.Get<UMAudio>().Effect.Play(GameAudio.Effect_2);
            });
            m_btnPlayEffect_3?.onClick.AddListener(() =>
            {
                // 播放 effect-3
                UMGR.Get<UMAudio>().Effect.Play(GameAudio.Effect_3);
            });

            m_togAudioEffect.onValueChanged.AddListener((val) =>
            {
                // 控制音效是否静音
                UMGR.Get<UMAudio>().Effect.Mute = val;
            });

            m_sldAudioEffect.onValueChanged.AddListener((val) =>
            {
                // 控制音效音量
                UMGR.Get<UMAudio>().Effect.Volume = val;
            });

            m_btnPrintASInfo?.onClick.AddListener(() =>
            {
                // 打印音效数量
                UMGR.Get<UMAudio>().Effect.PrintASInfo();
            });

            InitDataPerDebug();
            InitConfigDebug();
        }

        private void InitDataPerDebug()
        {
            string key = "tttteee";
            string val = "ddd-0000";
            string defaultVal = "dddffff";
            m_btnDataSave?.onClick.AddListener(() => { UMGR.Get<UMDataPer>().Save(key, val); });
            m_btnDataRead?.onClick.AddListener(() =>
            {
                string readVal = UMGR.Get<UMDataPer>().Read(key, defaultVal);
                Debug.Log($"Read Data Per: {readVal}");
            });
            m_btnDataDelete?.onClick.AddListener(() => { UMGR.Get<UMDataPer>().Delete(key); });
            m_btnDataDeleteAll?.onClick.AddListener(() => { UMGR.Get<UMDataPer>().DeleteAll(); });
        }

        private void InitConfigDebug()
        {
            string id = "bullet_89001";
            m_btnReadConfig?.onClick.AddListener(() =>
            {
                BulletData data = UMGR.Get<UMConfig>().GetTable<BulletTable>().GetDataById(id);
                Debug.Log($"configData: id: {data.id};  {data.bulletPath}");
            });
        }

        protected override void OnDestroyPanel()
        {
        }

        protected override void OnOpenPanel()
        {
            m_togAudioEffect.isOn = UMGR.Get<UMAudio>().Effect.Mute;
            m_sldAudioEffect.value = UMGR.Get<UMAudio>().Effect.Volume;
        }

        protected override void OnClosePanel()
        {
        }
    }
}