using System.Collections.Generic;
using Game.Scripts.Common;
using Game.Scripts.Common.GameUI;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.DataPer;
using UMiniFramework.Runtime.Modules.DataPer.UMDataPerHandlers;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.Scene;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UMDataPer;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Scene.Launch
{
    public class GameLaunch : MonoBehaviour
    {
        [SerializeField] private Slider m_slidLaunchProgress;
        [SerializeField] private Text m_txtProgressTip;

        private void Start()
        {
            m_txtProgressTip.text = string.Empty;

            // UI 配置
            UMUIInitArgs umUIConfig = new UMUIInitArgs();
            umUIConfig.IsCreateEventSystem = true;
            umUIConfig.UILayerCount = 5;

            // 音频配置
            UMAudioInitArgs umAudioConfig = new UMAudioInitArgs();
            umAudioConfig.BGMClips = new List<AudioClipInfo>()
            {
                new(GameAudio.BGM_1, "Audio/BGM/BGM_001"),
                new(GameAudio.BGM_2, "Audio/BGM/BGM_002", true),
            };

            // umAudioConfig.DefaultAsCount = 0;
            umAudioConfig.EffectClips = new List<AudioClipInfo>()
            {
                new(GameAudio.Effect_1, "Audio/Effect/Bullet_Explosion_001"),
                new(GameAudio.Effect_2, "Audio/Effect/Effect_Cannon_001", true),
                new(GameAudio.Effect_3, "Audio/Effect/Effect_Cannon_002"),
            };

            // 数据持久化配置
            UMDataPerInitArgs umDataPerConfig = new UMDataPerInitArgs();
            // umDataPerConfig.DataPerHandler = new UMDataUnityPrefsHandler();
            umDataPerConfig.DataPerHandler = new UMDataJsonFileHandler();

            UMGR.Launch();

            UMGR.Register<UMUI>(umUIConfig);
            UMGR.Register<UMAudio>(umAudioConfig);
            UMGR.Register<UMScene>();
            UMGR.Register<UMDataPer>(umDataPerConfig);

            UMGR.InitModules((val) =>
            {
                UMBaseModule module = val.InitModule;

                string moduleTypeStr = module == null ? "Finished" : module.ModuleType.ToString();

                m_txtProgressTip.text = $"Loading {moduleTypeStr}. {val.InitProgress * 100}%";

                Debug.Log($"Init modules progress: {val.InitProgress}. module: {moduleTypeStr}");
                m_slidLaunchProgress.value = val.InitProgress;
                if (val.InitState)
                {
                    OnUMGRInitModulesFinished();
                }
            });
        }

        private void OnUMGRInitModulesFinished()
        {
            GameUI.OpenDebug();
            // 进入主界面
            UMGR.Get<UMScene>().Load(GameScene.Main);
        }
    }
}