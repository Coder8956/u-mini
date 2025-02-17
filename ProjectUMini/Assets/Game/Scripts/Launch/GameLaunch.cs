using System.Collections.Generic;
using Game.Scripts.Const;
using Game.Scripts.PersistentData;
using UMiniFramework.Runtime.Modules.AssetModule.AssetLoaders;
using UMiniFramework.Runtime.Modules.ConfigModule;
using UMiniFramework.Runtime.Modules.PersistentDataModule;
using UMiniFramework.Runtime.UMEntrance;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Launch
{
    public class GameLaunch : MonoBehaviour
    {
        [SerializeField] private Button m_btnEnterGame;

        private void Awake()
        {
            m_btnEnterGame.gameObject.SetActive(false);
            m_btnEnterGame.onClick.AddListener(() => { UMini.Scene.Load(SceneConst.LOGIN); });
        }

        // Start is called before the first frame update
        void Start()
        {
            UMiniConfig umConfig = new UMiniConfig();
            umConfig.OnLaunchFinished = () =>
            {
                Debug.Log($"LaunchFinished >>> UMini Init　Finished: {UMini.UMInitFinished}");
                m_btnEnterGame.gameObject.SetActive(true);
                // UMini.UI.Interactable = true;
            };
            umConfig.LaunchProgress = (progressTag, progressVal) =>
            {
                Debug.Log($"UMini Init　Finished: {UMini.UMInitFinished}; " +
                          $"LaunchProgressTag: {progressTag}, " +
                          $"ProgressVal: {progressVal}.");
            };
            umConfig.AssetLoader = new UMResourcesLoader();
            umConfig.IsPersiDataToConsole = true;
            umConfig.IsLog = true;
            umConfig.ConfigTableList = new List<UMConfigTable>()
            {
                new LevelTable(),
                new GameAudioTable(),
                new CannonTable(),
                new BulletTable(),
                new MonsterTable(),
            };
            umConfig.PersistentDataList = new List<UMPersistentData>()
            {
                new UserData(),
                new GameData(),
                new UIData(),
            };
            umConfig.EventTypeList = new List<string>()
            {
                GameEventConst.Launch,
                GameEventConst.Login
            };
            UMini.Launch(umConfig);
        }
    }
}