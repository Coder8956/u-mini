using System;
using Game.Scripts.Common;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
using UnityEngine;

namespace Game.Scripts.Scene.Main
{
    public class GameMain : MonoBehaviour
    {
        private void Start()
        {
            UMGR.Get<UMUI>().Open(GameUI.PanelMain);
            UMGR.Get<UMAudio>().BGM.Play(GameAudio.BGM_1);
        }
    }
}