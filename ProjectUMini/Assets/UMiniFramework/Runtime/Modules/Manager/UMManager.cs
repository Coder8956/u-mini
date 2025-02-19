using System;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Manager
{
    public class UMManager : MonoBehaviour
    {
        private static UMManager m_UMGR;

        public static UMManager UMGR => m_UMGR;

        private UMGR_STATE state = UMGR_STATE.DEFAULT;

        private void Awake()
        {
            if (m_UMGR == null)
            {
                m_UMGR = GetComponent<UMManager>();
                DontDestroyOnLoad(gameObject);
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Init()
        {
            state = UMGR_STATE.DEFAULT;
            UMUtilDebug.Log($"Start init UMGR. State:{state}");

            state = UMGR_STATE.INITED;
            UMUtilDebug.Log($"UMGR init Finished. State:{state}");
        }
    }
}