using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Config;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
using UnityEngine;

namespace Game.Scripts.Launch
{
    public class GameLaunch : MonoBehaviour
    {
        private void Start()
        {
            UMGR.Launch();
            UMGR.Register<UMUI>();
            UMGR.Register<UMAudio>();
            UMGR.Register<UMConfig>();
            UMGR.InitModules((val) => { Debug.Log($"Init modules progress: {val.InitProgress}"); });
        }
    }
}