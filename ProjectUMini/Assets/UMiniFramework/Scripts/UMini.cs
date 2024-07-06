using System;
using System.Collections;
using System.Collections.Generic;
using UMiniFramework.Scripts.Kit;
using UMiniFramework.Scripts.Modules;
using UMiniFramework.Scripts.Modules.UI;
using UMiniFramework.Scripts.Modules.Audio;
using UMiniFramework.Scripts.Modules.Config;
using UMiniFramework.Scripts.Modules.Scene;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Scripts
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

        /// <summary>
        /// 场景模块
        /// </summary>
        public static UMSceneModule Scene { get; private set; }

        /// <summary>
        /// UI模块
        /// </summary>
        public static UMUIModule UI { get; private set; }

        private List<UMModule> m_moduleList = null;

        private UMiniConfig m_config = null;

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
                UMDebug.Warning("Destroy duplicate UMEntity instances on awake");
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
            UMDebug.Log(">>> UMini Launch");
            m_config = config;
            m_moduleList = new List<UMModule>();

            Audio = m_audioModule;
            m_moduleList.Add(Audio);

            StartCoroutine(InitModules());
        }

        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitModules()
        {
            foreach (var module in m_moduleList)
            {
                yield return module.Init();
            }
            UMDebug.Log(">>> UMini Launch Finished.");
            m_config?.OnLaunchFinished?.Invoke();
        }

        public class UMiniConfig
        {
            public Action OnLaunchFinished { get; set; }
        }
    }
}