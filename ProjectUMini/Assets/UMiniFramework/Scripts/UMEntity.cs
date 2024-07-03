using System;
using System.Collections;
using System.Collections.Generic;
using UMiniFramework.Scripts.Kit;
using UMiniFramework.Scripts.Modules;
using UnityEngine;

namespace UMiniFramework.Scripts
{
    public class UMEntity : MonoBehaviour
    {
        private static UMEntity GlobalInstance = null;

        /// <summary>
        /// 音频模块
        /// </summary>
        public static AudioModule Audio { get; private set; }

        /// <summary>
        /// 配置模块
        /// </summary>
        public static ConfigModule Config { get; private set; }

        /// <summary>
        /// 场景模块
        /// </summary>
        public static SceneModule Scene { get; private set; }

        /// <summary>
        /// UI模块
        /// </summary>
        public static UIModule UI { get; private set; }

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

        // Start is called before the first frame update
        void Start()
        {
            UMDebug.Log("UMEntity Start");
            StartCoroutine(InitModules(() => { Scene.Load("Login"); }));
        }

        private IEnumerator InitModules(Action OnCreated = null)
        {
            UMDebug.Log(">>> UMEntity Init Modules begin <<<");

            List<UMModule> modules = new List<UMModule>();

            // 创建音频模块
            if (CheckModuleIsNull(Audio))
            {
                Audio = UMTool.CreateGameObject<AudioModule>(gameObject);
                modules.Add(Audio);
            }

            // 创建配置模块
            if (CheckModuleIsNull(Config))
            {
                Config = UMTool.CreateGameObject<ConfigModule>(gameObject);
                modules.Add(Config);
            }

            // 创建场景模块
            if (CheckModuleIsNull(Scene))
            {
                Scene = UMTool.CreateGameObject<SceneModule>(gameObject);
                modules.Add(Scene);
            }

            // 创建UI模块
            if (CheckModuleIsNull(UI))
            {
                UI = UMTool.CreateGameObject<UIModule>(gameObject);
                modules.Add(UI);
            }

            foreach (var module in modules)
            {
                yield return module.Init();
            }

            UMDebug.Log(">>> UMEntity Init Modules end <<<");
            OnCreated?.Invoke();
        }

        private bool CheckModuleIsNull(UMModule module)
        {
            return module == null;
        }
    }
}