using System;
using Game.Scripts.UI.Game;
using UMiniFramework.Scripts;
using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class GameMain : MonoBehaviour
    {
        private void Start()
        {
            UMini.UI.Open<GamePanel>();
        }
    }
}