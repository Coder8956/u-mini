using Game.Scripts.Const;
using Game.Scripts.EventBody;
using Game.Scripts.PersistentData;
using Game.Scripts.UI.Dialog;
using Game.Scripts.UI.Login;
using UMiniFramework.Scripts.UMEntrance;
using UMiniFramework.Scripts.Modules.UIModule;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Debug
{
    [UMUIPanelInfo("UI/Debug/DebugPanel", UMUILayer.TOP)]
    public class DebugPanel : UMUIPanel
    {
        [SerializeField] private Button m_btnShowDebugFunc;
        [SerializeField] private RectTransform m_debugFuncRoot;

        [SerializeField] private Button m_btnOpenLogin;
        [SerializeField] private Button m_btnOpenDialog;
        [SerializeField] private Button m_btnDispatchEvent;

        #region AUDIO_DEBUG

        [SerializeField] private Button m_btnPlayBGM_1;
        [SerializeField] private Button m_btnPlayBGM_2;
        [SerializeField] private Button m_btnStopBGM;
        [SerializeField] private Button m_btnBGMMute;
        [SerializeField] private Button m_btnEffectAudio_1;
        [SerializeField] private Button m_btnEffectAudio_2;

        #endregion

        #region UMPersistentData_DEBUG

        [SerializeField] private Button m_btnUpdateUserData;

        #endregion

        public override void OnLoaded()
        {
            m_debugFuncRoot.gameObject.SetActive(false);
            m_btnShowDebugFunc.onClick.AddListener(() =>
            {
                m_debugFuncRoot.gameObject.SetActive(!m_debugFuncRoot.gameObject.activeSelf);
            });

            m_btnOpenLogin.onClick.AddListener(OpenLoginPanel);
            m_btnOpenDialog.onClick.AddListener(OpenDialog);
            m_btnDispatchEvent.onClick.AddListener(DispatchEvent);

            m_btnPlayBGM_1.onClick.AddListener(PlayBGM_1);
            m_btnPlayBGM_2.onClick.AddListener(PlayBGM_2);
            m_btnStopBGM.onClick.AddListener(StopBGM);
            m_btnBGMMute.onClick.AddListener(BGMMute);
            m_btnEffectAudio_1.onClick.AddListener(EffectAudio_1);
            m_btnEffectAudio_2.onClick.AddListener(EffectAudio_2);

            m_btnUpdateUserData.onClick.AddListener(UpdateUserData);
        }

        private void DispatchEvent()
        {
            UMini.Event.Dispatch(GameEventConst.Launch, new EBDebug("Launch"));
            // UMini.Event.Dispatch(GameEventConst.Login, new EBDebug("Login"));
        }

        private void OpenDialog()
        {
            UMini.UI.Open<DialogTip>();
        }

        private void UpdateUserData()
        {
            UMini.PersiData.Query<UserData>().Age = 15;
            UMini.PersiData.Query<UserData>().Name = "TestName_xxxx";

            UMini.PersiData.Query<UIData>().Scale = 105;
            UMini.PersiData.Query<UIData>().UIName = "UIData465";

            UMini.PersiData.Query<GameData>().Level = 315;
            UMini.PersiData.Query<GameData>().GameName = "GameData78979";

            UMini.PersiData.SaveAllData();
        }

        private void PlayBGM_1()
        {
            UMini.Audio.BGM.Play("Audio/BGM/BGM_001");
        }

        private void PlayBGM_2()
        {
            UMini.Audio.BGM.Play("Audio/BGM/BGM_002", 0.2f, false);
        }

        private void StopBGM()
        {
            UMini.Audio.BGM.Stop();
        }

        private void BGMMute()
        {
            UMini.Audio.BGM.SetMute(!UMini.Audio.BGM.GetMute());
        }

        private void EffectAudio_1()
        {
            UMini.Audio.Effect.Play("Audio/Effect/Effect_Cannon_001");
        }

        private void EffectAudio_2()
        {
            UMini.Audio.Effect.Play("Audio/Effect/Effect_Cannon_002");
        }

        private void OpenLoginPanel()
        {
            UMini.UI.Open<LoginPanel>();
        }

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {
        }
    }
}