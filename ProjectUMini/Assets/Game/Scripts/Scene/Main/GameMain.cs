using Game.Scripts.Common;
using UMiniFramework.Runtime.Modules.Manager;
using UnityEngine;

namespace Game.Scripts.Scene.Main
{
    public class GameMain : MonoBehaviour
    {
        private void Start()
        {
            GameUI.OpenMain();
            UMF.Audio.BGM.Play(GameAudio.BGM_Main);
            GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
    }
}