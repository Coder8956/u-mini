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
            UMGR.Launch();
            UMGR.Register<UMUI>();
            UMGR.Get<UMUI>();
        }
    }
}