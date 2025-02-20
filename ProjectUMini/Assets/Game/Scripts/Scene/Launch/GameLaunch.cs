using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Config;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
using UnityEngine;

namespace Game.Scripts.Scene.Launch
{
    public class GameLaunch : MonoBehaviour
    {
        private void Start()
        {
            UMUIConfig umuiConfig = new UMUIConfig();
            umuiConfig.IsCreateEventSystem = true;
            umuiConfig.UILayerCount = 5;


            UMGR.Launch();
            UMGR.Register<UMUI>(umuiConfig);
            UMGR.Register<UMAudio>();
            UMGR.Register<UMConfig>();
            UMGR.InitModules((val) => { Debug.Log($"Init modules progress: {val.InitProgress}"); });
        }
    }
}