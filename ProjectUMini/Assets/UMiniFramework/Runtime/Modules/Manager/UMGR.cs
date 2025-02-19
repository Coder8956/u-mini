using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.BaseModule;
using UMiniFramework.Runtime.Utils;
using UnityEngine;
using UnityEngine.WSA;

namespace UMiniFramework.Runtime.Modules.Manager
{
    public class UMGR : MonoBehaviour
    {
        private static UMGR m_instance;

        private static UMGR_STATE m_state = UMGR_STATE.INVALID;

        public static UMGR_STATE State => m_state;

        private static Dictionary<string, UMBaseModule> m_moduleDic;

        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = GetComponent<UMGR>();
                DontDestroyOnLoad(gameObject);
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private static void Init()
        {
            m_state = UMGR_STATE.INVALID;
            UMUtilDebug.Log($"Start init UMGR State:{m_state}");
            m_moduleDic = new Dictionary<string, UMBaseModule>();
            m_state = UMGR_STATE.INITED;
            UMUtilDebug.Log($"UMGR init Finished State:{m_state}");
        }

        public static void Launch()
        {
            m_state = UMGR_STATE.LAUNCHING;
            UMUtilDebug.Log($"UMGR State:{m_state}");

            m_state = UMGR_STATE.LAUNCHED;
            UMUtilDebug.Log($"UMGR State:{m_state}");
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