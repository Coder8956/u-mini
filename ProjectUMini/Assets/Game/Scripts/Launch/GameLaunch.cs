using System.Collections.Generic;
using Game.Scripts.Const;
using UMiniFramework.Scripts;
using UMiniFramework.Scripts.Modules.AssetModule.AssetLoaders;
using UMiniFramework.Scripts.Modules.ConfigModule;
using UMiniFramework.Scripts.Modules.PersistentDataModule;
using UMiniFramework.Scripts.Utils;
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
            UMini.UMiniConfig umConfig = new UMini.UMiniConfig();
            umConfig.OnLaunchFinished = () => { m_btnEnterGame.gameObject.SetActive(true); };
            umConfig.AssetLoader = new UMResourcesLoader();
            umConfig.DataConverter = new UMDefaultDataConverter();
            umConfig.DataPers = new UMDefaultDataPers();
            umConfig.IsPersiDataToConsole = true;
            umConfig.ConfigTableList = new List<UMConfigTable>()
            {
                new LevelTable(),
                new GameAudioTable(),
                new CannonTable(),
                new BulletTable(),
                new MonsterTable(),
            };
            UMini.Launch(umConfig);
        }
    }
}