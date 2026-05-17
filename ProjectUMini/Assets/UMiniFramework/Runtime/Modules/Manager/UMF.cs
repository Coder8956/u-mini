using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Manager
{
    public enum UMFState
    {
        /// <summary>
        /// 无效状态
        /// </summary>
        None,

        /// <summary>
        /// 正在启动
        /// </summary>
        Launching,

        /// <summary>
        /// 启动成功
        /// </summary>
        LaunchSuccessful
    }

    /// <summary>
    /// UM框架管理类
    /// </summary>
    public class UMF : MonoBehaviour
    {
        /// <summary>
        /// UMF状态
        /// </summary>
        private static UMFState State = UMFState.None;

        /// <summary>
        /// 框架实例
        /// </summary>
        private static UMF Instance = null;

        /// <summary>
        /// 框架实例游戏物体
        /// </summary>
        private static GameObject InstanceGo = null;

        private static string InstanceName = "UMF";

        /// <summary>
        /// 音频模块
        /// </summary>
        public static UMAudio Audio
        {
            get { return Instance.m_umAudio; }
        }

        private UMAudio m_umAudio;

        /// <summary>
        /// 配置模块
        /// </summary>
        public static UMConfig Config
        {
            get { return Instance.m_umConfig; }
        }

        private UMConfig m_umConfig;

        /// <summary>
        /// 数据模块
        /// </summary>
        public static UMDataPer Data
        {
            get { return Instance.m_umData; }
        }

        private UMDataPer m_umData;

        /// <summary>
        /// 事件模块
        /// </summary>
        public static UMEvent Event
        {
            get { return Instance.m_umEvent; }
        }

        private UMEvent m_umEvent;

        /// <summary>
        /// 对象池模块
        /// </summary>
        public static UMGOPools Pools
        {
            get { return Instance.m_umPools; }
        }

        private UMGOPools m_umPools;

        /// <summary>
        /// 本地化模块
        /// </summary>
        public static UMLocal Local
        {
            get { return Instance.m_umLocal; }
        }

        private UMLocal m_umLocal;

        /// <summary>
        /// 资源模块
        /// </summary>
        public static UMRes Res
        {
            get { return Instance.m_umRes; }
        }

        private UMRes m_umRes;

        /// <summary>
        /// 场景模块
        /// </summary>
        public static UMScene Scene
        {
            get { return Instance.m_umScene; }
        }

        private UMScene m_umScene;

        /// <summary>
        /// UI模块
        /// </summary>
        public static UMUI UI
        {
            get { return Instance.m_umUI; }
        }

        private UMUI m_umUI;

        private List<UMBaseModule> m_modules = new();

        private Action<UMFState> m_launchStateHandler = null;
        private Action<UMModuleType, float> m_launchProgressHandler;

        public static void Launch(
            Action<UMFState> launchStateHandler = null,
            Action<UMModuleType, float> launchProgressHandler = null
        )
        {
            if (Instance != null) return;
            State = UMFState.Launching;
            Instance = UMUtilCommon.CreateGameObject<UMF>(InstanceName, null);
            InstanceGo = Instance.gameObject;
            DontDestroyOnLoad(InstanceGo);

            Instance.m_launchProgressHandler = launchProgressHandler;
            Instance.m_launchStateHandler = launchStateHandler;

            Instance.m_launchStateHandler?.Invoke(State);

            Instance.m_umAudio = UMUtilCommon.CreateGameObject<UMAudio>(InstanceGo);
            Instance.m_modules.Add(Audio);

            Instance.m_umConfig = UMUtilCommon.CreateGameObject<UMConfig>(InstanceGo);
            Instance.m_modules.Add(Config);

            Instance.m_umData = UMUtilCommon.CreateGameObject<UMDataPer>(InstanceGo);
            Instance.m_modules.Add(Data);

            Instance.m_umEvent = UMUtilCommon.CreateGameObject<UMEvent>(InstanceGo);
            Instance.m_modules.Add(Event);

            Instance.m_umPools = UMUtilCommon.CreateGameObject<UMGOPools>(InstanceGo);
            Instance.m_modules.Add(Pools);

            Instance.m_umLocal = UMUtilCommon.CreateGameObject<UMLocal>(InstanceGo);
            Instance.m_modules.Add(Local);

            Instance.m_umRes = UMUtilCommon.CreateGameObject<UMRes>(InstanceGo);
            Instance.m_modules.Add(Res);

            Instance.m_umScene = UMUtilCommon.CreateGameObject<UMScene>(InstanceGo);
            Instance.m_modules.Add(Scene);

            Instance.m_umUI = UMUtilCommon.CreateGameObject<UMUI>(InstanceGo);
            Instance.m_modules.Add(UI);

            Instance.StartCoroutine(Instance.InitCoro());
        }

        private IEnumerator InitCoro()
        {
            for (var i = 0; i < m_modules.Count; i++)
            {
                UMBaseModule module = m_modules[i];
                Type moduleType = module.GetType();
                MethodInfo methodInitModule = UMUtilCommon.GetObjectNoPublicMethod(moduleType, "Init");
                m_launchProgressHandler?.Invoke(module.ModuleType, (float)(i + 1) / m_modules.Count);
                yield return methodInitModule.Invoke(module, null);
            }

            State = UMFState.LaunchSuccessful;
            m_launchStateHandler?.Invoke(State);
        }

        public static void DebugLog(bool val)
        {
            UMUtilDebug.Enable(val);
        }
    }
}