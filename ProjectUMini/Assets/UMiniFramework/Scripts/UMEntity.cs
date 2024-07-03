using System;
using System.Collections;
using System.Collections.Generic;
using UMiniFramework.Scripts.Kit;
using UMiniFramework.Scripts.Modules;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UMiniFramework.Scripts
{
    public class UMEntity : MonoBehaviour
    {
        private static UMEntity GlobalInstance = null;
        public bool EnableInit { get; private set; }

        private AudioModule m_audio = null;

        /// <summary>
        /// 音频模块
        /// </summary>
        public static AudioModule Audio
        {
            get { return GlobalInstance.m_audio; }
        }

        private ConfigModule m_config = null;

        /// <summary>
        /// 配置模块
        /// </summary>
        public static ConfigModule Config
        {
            get { return GlobalInstance.m_config; }
        }

        private SceneModule m_scene = null;

        /// <summary>
        /// 场景模块
        /// </summary>
        public static SceneModule Scene
        {
            get { return GlobalInstance.m_scene; }
        }

        private UIModule m_UI = null;

        /// <summary>
        /// UI模块
        /// </summary>
        public static UIModule UI
        {
            get { return GlobalInstance.m_UI; }
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
                Destroy(this);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            UMDebug.Log("UMEntity Start");
            EnableInit = true;
            // 创建音频模块
            // UMModule.CreateModule(ref m_audio, this);
            // 创建配置模块
            // UMModule.CreateModule(ref m_config, this);
            // 创建场景模块
            // UMModule.CreateModule(ref m_scene, this);
            // 创建UI模块
            // UMModule.CreateModule(ref m_UI, this);
            EnableInit = false;
            SceneManager.LoadScene("Login");
        }
    }
}