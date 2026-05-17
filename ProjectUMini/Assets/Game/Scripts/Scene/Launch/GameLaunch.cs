using System.Collections.Generic;
using Game.Scripts.Common;
using Game.Scripts.GameEvent;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Audio.ClipInfo;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Config;
using UMiniFramework.Runtime.Modules.Config.UMLoadConfigHandlers;
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
            
            UMF.InitModules(UMGRMIPHandler);
        }

        // private void UMGRMIPHandler(InitProgressInfo info)
        // {
        //     UMBaseModule module = info.InitModule;
        //     float initProgress = info.InitProgress;
        //
        //     if (!info.InitState)
        //     {
        //         UMModuleType moduleType = module.ModuleType;
        //         string moduleTypeStr = moduleType.ToString();
        //         // Debug.Log($"Init modules progress: {info.InitProgress}. module: {moduleTypeStr}");
        //         UpdateInitProgressUI(moduleTypeStr, initProgress);
        //     }
        //     else
        //     {
        //         // 处理初始化完成的状态
        //         // Debug.Log($"Init modules progress: {info.InitProgress}. modules init finished.");
        //         UpdateInitProgressUI("Finished", initProgress);
        //         GameLaunchFunc();
        //     }
        // }

        private void GameLaunchFunc()
        {
            GameAudioTable gameAudioTable = UMF.Get<UMConfig>().GetTable<GameAudioTable>();
            for (var i = 0; i < gameAudioTable.TableData.Count; i++)
            {
                GameAudioData gad = gameAudioTable.TableData[i];
                UMAudioClipInfo aci = new UMAudioClipInfo(gad.id, gad.path);
                switch (gad.type)
                {
                    case 0:
                        UMF.Get<UMAudio>().BGM.AddAudioClip(aci);
                        break;
                    case 1:
                        UMF.Get<UMAudio>().Effect.AddAudioClip(aci);
                        break;
                }
            }

            // string audioBGMMute = UMGR.Get<UMDataPer>().Read(GameDataPerKey.AudioBGMMute, false.ToString());
            // UMGR.Get<UMAudio>().BGM.Mute = bool.Parse(audioBGMMute);
            // string audioBGMVolume = UMGR.Get<UMDataPer>().Read(GameDataPerKey.AudioBGMVolume, "1");
            // UMGR.Get<UMAudio>().BGM.Volume = float.Parse(audioBGMVolume);
            //
            // string audioEffectMute = UMGR.Get<UMDataPer>().Read(GameDataPerKey.AudioEffectMute, false.ToString());
            // UMGR.Get<UMAudio>().Effect.Mute = bool.Parse(audioEffectMute);
            // string audioEffectVolume = UMGR.Get<UMDataPer>().Read(GameDataPerKey.AudioEffectVolume, "1");
            // UMGR.Get<UMAudio>().Effect.Volume = float.Parse(audioEffectVolume);

            UMF.Get<UMUI>().CanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            UMF.Get<UMUI>().CanvasScaler.referenceResolution = new Vector2(3840, 2160);
            UMF.Get<UMUI>().CanvasScaler.matchWidthOrHeight = 1; // 按高适配=1

            GameUI.OpenDebug();
            // 进入主界面
            UMF.Get<UMScene>().Load(GameScene.Main);
        }

        private void UpdateInitProgressUI(string tip, float progressVal)
        {
            m_txtProgressTip.text = $"Loading {tip}. {progressVal * 100}%";
            m_slidLaunchProgress.value = progressVal;
        }
    }
}