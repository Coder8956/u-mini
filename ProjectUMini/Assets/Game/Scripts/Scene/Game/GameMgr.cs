using System;
using Game.Scripts.Common;
using Game.Scripts.Common.GameUI;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Config;
using UMiniFramework.Runtime.Modules.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Scene.Game
{
    public class GameMgr : MonoBehaviour
    {
        private LevelData m_levelData = null;

        private void Start()
        {
            m_levelData = UMGR.Get<UMConfig>().GetTable<LevelTable>().GetDataById(GameGlobalVar.SelectLevelId);
            UMGR.Get<UMAudio>().BGM.Play(m_levelData.bgmId);
            GameUI.OpenGame();
        }
    }
}