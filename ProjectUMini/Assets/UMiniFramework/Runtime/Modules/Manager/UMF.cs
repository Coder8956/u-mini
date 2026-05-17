using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime.Modules.Base;
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
        StartingUp,

        /// <summary>
        /// 启动成功
        /// </summary>
        StartupSuccessful
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

        public static void Launch()
        {
            if (Instance != null) return;
            State = UMFState.StartingUp;
            Instance = UMUtilCommon.CreateGameObject<UMF>(InstanceName, null);
            InstanceGo = Instance.gameObject;
            DontDestroyOnLoad(InstanceGo);
            State = UMFState.StartupSuccessful;
        }

        public static void DebugLog(bool val)
        {
            UMUtilDebug.Enable(val);
        }
    }
}