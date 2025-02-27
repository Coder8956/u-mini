using System.Collections.Generic;
using Game.Scripts.Common;
using Game.Scripts.GameEvent;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Config;
using UMiniFramework.Runtime.Modules.Config.Base;
using UMiniFramework.Runtime.Modules.Config.UMLoadConfigHandlers;
using UMiniFramework.Runtime.Modules.DataPer;
using UMiniFramework.Runtime.Modules.DataPer.UMDataPerHandlers;
using UMiniFramework.Runtime.Modules.Event;
using UMiniFramework.Runtime.Modules.GOPools;
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

            // 数据持久化配置
            UMDataPerInitArgs umDataPerInitArgs = new UMDataPerInitArgs();
            umDataPerInitArgs.DataPerHandler = new UMDataJsonFileHandler();

            UMConfigInitArgs umConfigInitArgs = new UMConfigInitArgs();
            umConfigInitArgs.LoadConfigHandler = new UMResLoadConfigHandler();
            umConfigInitArgs.ConfigTables = new List<UMConfigTable>();

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

            GameUI.OpenDebug();
            // 进入主界面
            UMGR.Get<UMScene>().Load(GameScene.Main);
        }
    }
}