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
            m_UMGRGameObject = new GameObject(UMGR_GO_NAME);
            DontDestroyOnLoad(m_UMGRGameObject);
            m_umgrInstance = m_UMGRGameObject.AddComponent<UMGR>();
            m_moduleDic = new Dictionary<string, UMBaseModule>();
            m_globalLaunched = true;
            UMUtilDebug.Log($"UMGR Launched.");
        }

        private static string GetModuleKey<T>() where T : UMBaseModule
        {
            return typeof(T).FullName;
        }

        public static void Register<T>() where T : UMBaseModule
        {
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