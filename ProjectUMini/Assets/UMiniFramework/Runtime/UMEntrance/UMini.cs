using System.Collections;
using System.Collections.Generic;
using UMiniFramework.Runtime.Modules;
using UMiniFramework.Runtime.Modules.AssetModule;
using UMiniFramework.Runtime.Modules.AudioModule;
using UMiniFramework.Runtime.Modules.ConfigModule;
using UMiniFramework.Runtime.Modules.EventModule;
using UMiniFramework.Runtime.Modules.PersistentDataModule;
using UMiniFramework.Runtime.Modules.SceneModule;
using UMiniFramework.Runtime.Modules.UIModule;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.UMEntrance
{
    public class UMini : MonoBehaviour
    {
        private static UMini GlobalInstance = null;

        /// <summary>
        /// 音频模块
        /// </summary>
        public static UMAudioModule Audio { get; private set; }

        [SerializeField] private UMAudioModule m_audioModule = null;

        /// <summary>
        /// 配置模块
        /// </summary>
        public static UMConfigModule Config { get; private set; }

        [SerializeField] private UMConfigModule m_configModule = null;

        /// <summary>
        /// 数据持久化模块
        /// </summary>
        public static UMPersistentDataModule PersiData { get; private set; }

        [SerializeField] private UMPersistentDataModule m_persiDataModule = null;

        /// <summary>
        /// 场景模块
        /// </summary>
        public static UMSceneModule Scene { get; private set; }

        [SerializeField] private UMSceneModule m_sceneModule = null;

        /// <summary>
        /// 资源模块
        /// </summary>
        public static UMAssetModule Asset { get; private set; }

        [SerializeField] private UMAssetModule m_assetModule = null;

        /// <summary>
        /// 消息模块
        /// </summary>
        public static UMEventModule Event { get; private set; }

        [SerializeField] private UMEventModule m_eventModule = null;

        /// <summary>
        /// UI模块
        /// </summary>
        public static UMUIModule UI { get; private set; }

        [SerializeField] private UMUIModule m_UIModule = null;

        private List<UMModule> m_moduleList = null;

        private UMiniConfig m_config = null;

        private static bool m_UMInitFinished = false;

        /// <summary>
        /// UM框架初始化完成
        /// </summary>
        public static bool UMInitFinished
        {
            get { return m_UMInitFinished; }
        }

        private void Awake()
        {
            if (GlobalInstance == null)
            {
                GlobalInstance = this;
                DontDestroyOnLoad(GlobalInstance);
            }
            else
            {
                Destroy(gameObject);
                UMUtilDebug.Warning("Destroy duplicate UMEntity instances on awake");
            }
        }

        /// <summary>
        /// 启动UMini框架 不能在MonoBehaviour Awake函数中调用 建议在Start函数中调用
        /// </summary>
        public static void Launch(UMiniConfig config = null)
        {
            GlobalInstance.InitFramework(config);
        }

        private void InitFramework(UMiniConfig config)
        {
            UMUtilDebug.PrintLog(config.IsLog);
            UMUtilDebug.Log(">>> UMini Launch");
            m_config = config;
            m_moduleList = new List<UMModule>();

            Audio = m_audioModule;
            Audio.InitPriority = 0;
            m_moduleList.Add(Audio);

            Scene = m_sceneModule;
            Scene.InitPriority = 0;
            m_moduleList.Add(Scene);

            Config = m_configModule;
            Config.InitPriority = 0;
            m_moduleList.Add(Config);

            PersiData = m_persiDataModule;
            PersiData.InitPriority = 0;
            m_moduleList.Add(PersiData);

            Asset = m_assetModule;
            Asset.InitPriority = -1;
            m_moduleList.Add(Asset);

            Event = m_eventModule;
            Event.InitPriority = 0;
            m_moduleList.Add(Event);

            UI = m_UIModule;
            UI.InitPriority = 0;
            m_moduleList.Add(UI);

            m_moduleList.Sort((x, y) => { return x.InitPriority > y.InitPriority ? 1 : -1; });

            // Debug.Log(m_moduleList);

            StartCoroutine(InitModules());
        }

        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitModules()
        {
            int initStepCount = m_moduleList.Count + 1;
            int initStepProgress = 0;
            string progressTag = string.Empty;
            foreach (var module in m_moduleList)
            {
                initStepProgress++;
                float progressVal = (float) initStepProgress / initStepCount;
                string moduleName = module.GetType().Name;
                progressTag = $"Init_{moduleName}";
                UMUtilDebug.Log($"UMiniInitModules => {progressTag}; InitFinishedVal: {module.InitFinished}");
                m_config.LaunchProgress?.Invoke(progressTag, progressVal);
                yield return module.Init(m_config);
            }

            // 初始化完成后执行的代码
            UMUtilDebug.Log($">>> UMini Launch Finished.");
            m_UMInitFinished = true;
            progressTag = "UMiniLaunchFinished";
            m_config.LaunchProgress?.Invoke(progressTag, 1);
            m_config?.OnLaunchFinished?.Invoke();
        }
    }
}