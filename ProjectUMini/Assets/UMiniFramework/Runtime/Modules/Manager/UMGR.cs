using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Manager
{
    public class UMGR : MonoBehaviour
    {
        /// <summary>
        /// 全局启动标记
        /// </summary>
        private static bool m_globalLaunched = false;

        private static UMGR m_umgrInstance;

        private const string UMGR_GO_NAME = "UMini-UMGR";

        private static GameObject m_UMGRGameObject = null;

        private static Dictionary<string, ModuleRegisterInfo> m_moduleDic = null;

        private IEnumerator InitModulesCoro(Action<InitProgressInfo> initCallback)
        {
            InitProgressInfo initInfo = new InitProgressInfo();
            initInfo.InitState = false;
            float moduleCount = m_moduleDic.Count;
            int initedNum = 0;
            MethodInfo ModuleInitMethod = null;

            foreach (var ele in m_moduleDic)
            {
                ModuleRegisterInfo registerInfo = ele.Value;
                initInfo.InitModule = registerInfo.Module;
                initInfo.InitProgress = initedNum / moduleCount;
                initCallback?.Invoke(initInfo);
                Type moduleType = registerInfo.Module.GetType();
                ModuleInitMethod = UMUtilCommon.GetObjectNoPublicMethod(moduleType, "Init");
                yield return ModuleInitMethod.Invoke(registerInfo.Module, new object[] {registerInfo.Config});
                initedNum++;
            }

            initInfo.InitModule = null;
            initInfo.InitProgress = initedNum / moduleCount;
            initInfo.InitState = true;
            initCallback?.Invoke(initInfo);
        }

        private static string GetModuleKey<T>() where T : UMBaseModule
        {
            return typeof(T).Name;
        }

        public static void Launch()
        {
            if (m_globalLaunched) return;
            m_umgrInstance = UMUtilCommon.CreateGameObject<UMGR>(UMGR_GO_NAME, null);
            m_UMGRGameObject = m_umgrInstance.gameObject;
            DontDestroyOnLoad(m_UMGRGameObject);
            m_moduleDic = new Dictionary<string, ModuleRegisterInfo>();
            m_globalLaunched = true;
            UMUtilDebug.Log($"UMGR Launched.");
        }

        public static void Register<T>(UMModuleConfig config = null) where T : UMBaseModule
        {
            string key = GetModuleKey<T>();
            if (m_moduleDic.ContainsKey(key))
            {
                UMUtilDebug.Warning($"Incorrect operation. The {key} was registered repeatedly.");
            }
            else
            {
                T module = UMUtilCommon.CreateGameObject<T>(key, m_UMGRGameObject);
                ModuleRegisterInfo registerInfo = new ModuleRegisterInfo(module, config);
                m_moduleDic.Add(key, registerInfo);
                UMUtilDebug.Log($"UMGR register module: {key}.");
            }
        }

        public static void InitModules(Action<InitProgressInfo> initCallback)
        {
            m_umgrInstance.StartCoroutine(m_umgrInstance.InitModulesCoro(initCallback));
        }

        public static T Get<T>() where T : UMBaseModule
        {
            string key = GetModuleKey<T>();
            if (m_moduleDic.ContainsKey(key))
            {
                return m_moduleDic[key].Module as T;
            }

            UMUtilDebug.Warning($"UMGR The {key} module is not registered");

            return null;
        }
    }
}