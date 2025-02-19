using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Manager
{
    public class UMManager : MonoBehaviour
    {
        private static UMManager m_UMGR;

        public static UMManager UMGR => m_UMGR;

        private UMGR_STATE m_state = UMGR_STATE.DEFAULT;

        public UMGR_STATE State => m_state;

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
            m_state = UMGR_STATE.DEFAULT;
            UMUtilDebug.Log($"Start init UMGR. State:{m_state}");

            m_state = UMGR_STATE.INITED;
            UMUtilDebug.Log($"UMGR init Finished. State:{m_state}");
        }
    }
}