using Game.Scripts.Common;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.GOPools;
using UMiniFramework.Runtime.Modules.Manager;
using UnityEngine;

namespace Game.Scripts.Scene.Main
{
    public class GameMain : MonoBehaviour
    {
        private void Start()
        {
            GameUI.OpenMain();
            UMGR.Get<UMAudio>().BGM.Play(GameAudio.BGM_Main);
            GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
    }
}