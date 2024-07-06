using UMiniFramework.Scripts;
using UnityEngine;

namespace Game.Scripts.Launch
{
    public class GameLaunch : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            UMini.UMiniConfig umConfig = new UMini.UMiniConfig();
            umConfig.OnLaunchFinished = () => { UMini.Scene.Load("Login"); };
            UMini.Launch(umConfig);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}