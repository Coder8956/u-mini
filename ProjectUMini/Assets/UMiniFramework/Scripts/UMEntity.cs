using UMiniFramework.Scripts.Kit;
using UMiniFramework.Scripts.Modules;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

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
            CreateModules();
            Scene.Load("Login");
        }

        private void CreateModules()
        {
            // 创建音频模块
            if (CheckModuleIsNull(Audio))
            {
                Audio = UMModuleTool.CreateModule<AudioModule>(this);
            }

            // 创建配置模块
            if (CheckModuleIsNull(Config))
            {
                Config = UMModuleTool.CreateModule<ConfigModule>(this);
            }

            // 创建场景模块
            if (CheckModuleIsNull(Scene))
            {
                Scene = UMModuleTool.CreateModule<SceneModule>(this);
            }

            // 创建UI模块
            if (CheckModuleIsNull(UI))
            {
                UI = UMModuleTool.CreateModule<UIModule>(this);
            }
        }

        private bool CheckModuleIsNull(UMModule module)
        {
            return module == null;
        }
    }
}