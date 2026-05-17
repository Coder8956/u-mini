using Game.Scripts.Common;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules;
using UMiniFramework.Runtime.Modules.Manager;
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

            UMF.Launch(UMFLaunchStateHandler, UMFLaunchProgressHandler);
        }

        private void UMFLaunchProgressHandler(UMModuleType type, float progress)
        {
            UpdateInitProgressUI(type.ToString(), progress);
        }

        private void UMFLaunchStateHandler(UMFState state)
        {
            if (state == UMFState.LaunchSuccessful)
            {
                // 添加配置表
                UMF.Config.AddTable(new BlockTable());
                UMF.Config.AddTable(new BulletTable());
                UMF.Config.AddTable(new LevelTable());
                UMF.Config.AddTable(new GameAudioTable());

                // 添加事件
                UMF.Event.AddEvent(GameEventTags.AddScore);
                UMF.Event.AddEvent(GameEventTags.GameAgain);
                UMF.Event.AddEvent(GameEventTags.AddShootCount);

                // 设置UI
                UMF.UI.PanelMaskColor = Color.white;

                GameLaunchFunc();
            }
        }

        private void GameLaunchFunc()
        {
            GameAudioTable gameAudioTable = UMF.Config.GetTable<GameAudioTable>();
            for (var i = 0; i < gameAudioTable.TableData.Count; i++)
            {
                GameAudioData gad = gameAudioTable.TableData[i];
                UMAudioClipInfo aci = new UMAudioClipInfo(gad.id, gad.path);
                switch (gad.type)
                {
                    case 0:
                        UMF.Audio.BGM.AddAudioClip(aci);
                        break;
                    case 1:
                        UMF.Audio.Effect.AddAudioClip(aci);
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

            UMF.UI.CanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            UMF.UI.CanvasScaler.referenceResolution = new Vector2(3840, 2160);
            UMF.UI.CanvasScaler.matchWidthOrHeight = 1; // 按高适配=1

            GameUI.OpenDebug();
            // 进入主界面
            UMF.Scene.Load(GameScene.Main);
        }

        private void UpdateInitProgressUI(string tip, float progressVal)
        {
            m_txtProgressTip.text = $"Loading {tip}. {progressVal * 100}%";
            m_slidLaunchProgress.value = progressVal;
        }
    }
}