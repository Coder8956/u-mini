using System.Collections.Generic;
using Game.Scripts.Common;
using Game.Scripts.Common.GameUI;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Config;
using UMiniFramework.Runtime.Modules.Config.Base;
using UMiniFramework.Runtime.Modules.Config.UMLoadConfigHandlers;
using UMiniFramework.Runtime.Modules.DataPer;
using UMiniFramework.Runtime.Modules.DataPer.UMDataPerHandlers;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.Resource;
using UMiniFramework.Runtime.Modules.Resource.UMResHandlers;
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
            UMUIInitArgs umUIInitArgs = new UMUIInitArgs();
            umUIInitArgs.IsCreateEventSystem = true;
            umUIInitArgs.UILayerCount = 5;

            // 音频配置
            UMAudioInitArgs umAudioInitArgs = new UMAudioInitArgs();
            umAudioInitArgs.BGMClips = new List<AudioClipInfo>()
            {
                new(GameAudio.BGM_1, "Audio/BGM/BGM_001"),
                new(GameAudio.BGM_2, "Audio/BGM/BGM_002", true),
            };
            umAudioInitArgs.EffectClips = new List<AudioClipInfo>()
            {
                new(GameAudio.Effect_1, "Audio/Effect/Bullet_Explosion_001"),
                new(GameAudio.Effect_2, "Audio/Effect/Effect_Cannon_001", true),
                new(GameAudio.Effect_3, "Audio/Effect/Effect_Cannon_002"),
            };

            // 数据持久化配置
            UMDataPerInitArgs umDataPerInitArgs = new UMDataPerInitArgs();
            umDataPerInitArgs.DataPerHandler = new UMDataJsonFileHandler();

            UMConfigInitArgs umConfigInitArgs = new UMConfigInitArgs();
            umConfigInitArgs.LoadConfigHandler = new UMResLoadConfigHandler();
            umConfigInitArgs.ConfigTables = new List<UMConfigTable>();

            umConfigInitArgs.ConfigTables.Add(new BulletTable());
            umConfigInitArgs.ConfigTables.Add(new CannonTable());
            umConfigInitArgs.ConfigTables.Add(new LevelTable());
            umConfigInitArgs.ConfigTables.Add(new MonsterTable());
            umConfigInitArgs.ConfigTables.Add(new GameAudioTable());

            // 资源加载配置
            UMResInitArgs umResourceInitArgs = new UMResInitArgs();
            umResourceInitArgs.ResHandler = new UMResDefaultHandler();

            UMGR.Launch();
            UMGR.Register<UMUI>(umUIInitArgs);
            UMGR.Register<UMAudio>(umAudioInitArgs);
            UMGR.Register<UMScene>();
            UMGR.Register<UMDataPer>(umDataPerInitArgs);
            UMGR.Register<UMConfig>(umConfigInitArgs);
            UMGR.Register<UMRes>(umResourceInitArgs);

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