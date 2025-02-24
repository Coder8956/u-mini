using System;
using Game.Scripts.Common;
using Game.Scripts.Common.GameUI;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Manager;
using UnityEngine;

namespace Game.Scripts.Scene.Game
{
    public class GameMgr : MonoBehaviour
    {
        private void Start()
        {
            UMGR.Get<UMAudio>().BGM.Play(GameAudio.BGM_2);
            GameUI.OpenGame();
        }
    }
}