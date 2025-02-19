using System.Collections;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
using UnityEngine;

namespace Game.Scripts.Launch
{
    public class GameLaunch : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(WaitUMGRInited());
        }

        private IEnumerator WaitUMGRInited()
        {
            yield return new WaitUntil(() => UMGR.State == UMGR_STATE.INITED);
            Debug.Log("Can use UMGR");
            UMGR.Register<UMUI>();
            UMGR.Launch();
            UMGR.Get<UMUI>();
        }
    }
}