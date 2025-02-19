using System.Collections.Generic;
using System.Data.SqlTypes;
using UMiniFramework.Runtime.Modules.BaseModule;
using UMiniFramework.Runtime.Utils;
using UnityEngine;
using UnityEngine.WSA;

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

        private static Dictionary<string, UMBaseModule> m_moduleDic = null;

        public static void Launch()
        {
            if (m_globalLaunched) return;
            m_umgrInstance = UMUtilCommon.CreateGameObject<UMGR>(UMGR_GO_NAME, null);
            m_UMGRGameObject = m_umgrInstance.gameObject;
            DontDestroyOnLoad(m_UMGRGameObject);
            m_moduleDic = new Dictionary<string, UMBaseModule>();
            m_globalLaunched = true;
            UMUtilDebug.Log($"UMGR Launched.");
        }

        private static string GetModuleKey<T>() where T : UMBaseModule
        {
            return typeof(T).Name;
        }

        public static void Register<T>() where T : UMBaseModule
        {
            string key = GetModuleKey<T>();
            if (m_moduleDic.ContainsKey(key))
            {
                UMUtilDebug.Warning($"Incorrect operation. The {key} was registered repeatedly.");
            }
            else
            {
                T module = UMUtilCommon.CreateGameObject<T>(key, m_UMGRGameObject);
                m_moduleDic.Add(key, module);
                UMUtilDebug.Log($"UMGR register module: {key}.");
            }
        }

        public static T Get<T>() where T : UMBaseModule
        {
            string key = GetModuleKey<T>();
            if (m_moduleDic.ContainsKey(key))
            {
                return m_moduleDic[key] as T;
            }

            UMUtilDebug.Warning($"UMGR The {key} module is not registered");

            return null;
        }
    }
}