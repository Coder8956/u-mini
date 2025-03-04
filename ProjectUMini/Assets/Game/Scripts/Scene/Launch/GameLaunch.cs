using System.Collections.Generic;
using Game.Scripts.Common;
using Game.Scripts.GameEvent;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Audio.ClipInfo;
using UMiniFramework.Runtime.Modules.Audio.InitArgs;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Config;
using UMiniFramework.Runtime.Modules.Config.InitArgs;
using UMiniFramework.Runtime.Modules.Config.UMLoadConfigHandlers;
using UMiniFramework.Runtime.Modules.DataPer.InitArgs;
using UMiniFramework.Runtime.Modules.DataPer.UMDataPerHandlers;
using UMiniFramework.Runtime.Modules.Event;
using UMiniFramework.Runtime.Modules.Event.InitArgs;
using UMiniFramework.Runtime.Modules.GOPools;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.Manager.Info;
using UMiniFramework.Runtime.Modules.Resource;
using UMiniFramework.Runtime.Modules.Resource.InitArgs;
using UMiniFramework.Runtime.Modules.Resource.UMResHandlers;
using UMiniFramework.Runtime.Modules.Scene;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.InitArgs;
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

            // 数据持久化配置
            UMDataPerInitArgs umDataPerInitArgs = new UMDataPerInitArgs();
            umDataPerInitArgs.DataPerHandler = new UMDataJsonFileHandler();

            UMConfigInitArgs umConfigInitArgs = new UMConfigInitArgs();
            umConfigInitArgs.LoadConfigHandler = new UMResLoadConfigHandler();

            umConfigInitArgs.ConfigTables.Add(new BulletTable());
            umConfigInitArgs.ConfigTables.Add(new LevelTable());
            umConfigInitArgs.ConfigTables.Add(new GameAudioTable());
            umConfigInitArgs.ConfigTables.Add(new BlockTable());

            // 资源加载配置
            UMResInitArgs umResourceInitArgs = new UMResInitArgs();
            umResourceInitArgs.ResHandler = new UMResDefaultHandler();

            UMEventInitArgs umEventInitArgs = new UMEventInitArgs();
            umEventInitArgs.RegisterEventTags = new List<string>();
            umEventInitArgs.RegisterEventTags.Add(GameEventTags.AddScore);
            umEventInitArgs.RegisterEventTags.Add(GameEventTags.AddShootCount);
            umEventInitArgs.RegisterEventTags.Add(GameEventTags.GameAgain);

            UMGR.Launch();
            UMGR.Register<UMUI>(umUIInitArgs);
            UMGR.Register<UMAudio>(umAudioInitArgs);
            UMGR.Register<UMScene>();
            UMGR.Register<UMDataPer>(umDataPerInitArgs);
            UMGR.Register<UMConfig>(umConfigInitArgs);
            UMGR.Register<UMRes>(umResourceInitArgs);
            UMGR.Register<UMEvent>(umEventInitArgs);
            UMGR.Register<UMGOPools>();

            // 测试不传参数
            // UMGR.Register<UMUI>();
            // UMGR.Register<UMAudio>();
            // UMGR.Register<UMScene>();
            // UMGR.Register<UMDataPer>();
            // UMGR.Register<UMConfig>();
            // UMGR.Register<UMRes>();
            // UMGR.Register<UMEvent>();
            // UMGR.Register<UMGOPools>();

            UMGR.InitModules(UMGRMIPHandler);
        }

        private void UMGRMIPHandler(InitProgressInfo info)
        {
            UMBaseModule module = info.InitModule;
            float initProgress = info.InitProgress;

            if (!info.InitState)
            {
                UMModuleType moduleType = module.ModuleType;
                string moduleTypeStr = moduleType.ToString();
                Debug.Log($"Init modules progress: {info.InitProgress}. module: {moduleTypeStr}");
                UpdateInitProgressUI(moduleTypeStr, initProgress);
            }
            else
            {
                // 处理初始化完成的状态
                Debug.Log($"Init modules progress: {info.InitProgress}. modules init finished.");
                UpdateInitProgressUI("Finished", initProgress);
                GameLaunchFunc();
            }
        }

        private void GameLaunchFunc()
        {
            GameAudioTable gameAudioTable = UMGR.Get<UMConfig>().GetTable<GameAudioTable>();
            for (var i = 0; i < gameAudioTable.TableData.Count; i++)
            {
                GameAudioData gad = gameAudioTable.TableData[i];
                UMAudioClipInfo aci = new UMAudioClipInfo(gad.id, gad.path);
                switch (gad.type)
                {
                    case 0:
                        UMGR.Get<UMAudio>().BGM.AddAudioClip(aci);
                        break;
                    case 1:
                        UMGR.Get<UMAudio>().Effect.AddAudioClip(aci);
                        break;
                }
            }

            string audioBGMMute = UMGR.Get<UMDataPer>().Read(GameDataPerKey.AudioBGMMute, false.ToString());
            UMGR.Get<UMAudio>().BGM.Mute = bool.Parse(audioBGMMute);
            string audioBGMVolume = UMGR.Get<UMDataPer>().Read(GameDataPerKey.AudioBGMVolume, "1");
            UMGR.Get<UMAudio>().BGM.Volume = float.Parse(audioBGMVolume);

            string audioEffectMute = UMGR.Get<UMDataPer>().Read(GameDataPerKey.AudioEffectMute, false.ToString());
            UMGR.Get<UMAudio>().Effect.Mute = bool.Parse(audioEffectMute);
            string audioEffectVolume = UMGR.Get<UMDataPer>().Read(GameDataPerKey.AudioEffectVolume, "1");
            UMGR.Get<UMAudio>().Effect.Volume = float.Parse(audioEffectVolume);

            UMGR.Get<UMUI>().CanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            UMGR.Get<UMUI>().CanvasScaler.referenceResolution = new Vector2(3840, 2160);
            UMGR.Get<UMUI>().CanvasScaler.matchWidthOrHeight = 1; // 按高适配=1

            GameUI.OpenDebug();
            // 进入主界面
            UMGR.Get<UMScene>().Load(GameScene.Main);
        }

        private void UpdateInitProgressUI(string tip, float progressVal)
        {
            m_txtProgressTip.text = $"Loading {tip}. {progressVal * 100}%";
            m_slidLaunchProgress.value = progressVal;
        }
    }
}