using System.Collections.Generic;
using Game.Scripts.Common;
using Game.Scripts.UI.PanelDebug;
using Game.Scripts.UI.PanelMain;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
using UnityEngine;

namespace Game.Scripts.Scene.Launch
{
    public class GameLaunch : MonoBehaviour
    {
        private void Start()
        {
            // UI 配置
            UMUIConfig umUIConfig = new UMUIConfig();
            umUIConfig.IsCreateEventSystem = true;
            umUIConfig.UILayerCount = 5;

            // 音频配置
            UMAudioConfig umAudioConfig = new UMAudioConfig();
            umAudioConfig.BGMClips = new List<AudioClipInfo>()
            {
                new(GameAudio.BGM_1, "Audio/BGM/BGM_001"),
                new(GameAudio.BGM_2, "Audio/BGM/BGM_002", true),
            };

            umAudioConfig.DefaultAsCount = 6;
            umAudioConfig.EffectClips = new List<AudioClipInfo>()
            {
                new(GameAudio.Effect_1, "Audio/Effect/Bullet_Explosion_001"),
                new(GameAudio.Effect_2, "Audio/Effect/Effect_Cannon_001", true),
                new(GameAudio.Effect_3, "Audio/Effect/Effect_Cannon_002"),
            };


            UMGR.Launch();
            UMGR.Register<UMUI>(umUIConfig);
            UMGR.Register<UMAudio>(umAudioConfig);
            // UMGR.Register<UMConfig>();
            UMGR.InitModules((val) =>
            {
                Debug.Log($"Init modules progress: {val.InitProgress}");
                if (val.InitState)
                {
                    OnUMGRInitModulesFinished();
                }
            });
        }

        private void OnUMGRInitModulesFinished()
        {
            // PanelGame pGame = UMGR.Get<UMUI>().Create<PanelGame>();
            GameUI.PanelMain = UMGR.Get<UMUI>().Create<PanelMain>();
            PanelDebug pDebug = UMGR.Get<UMUI>().Create<PanelDebug>();

            UMGR.Get<UMUI>().Open(pDebug);
        }
    }
}