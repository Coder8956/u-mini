using System;
using Game.Scripts.UI.Game;
using UMiniFramework.Scripts;
using UMiniFramework.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class GameMain : MonoBehaviour
    {
        private static string GameLevelId = string.Empty;
        private LevelData m_levelData;
        private GameAudioData m_gameAudioData;

        public static void SetGameLevel(string levelId)
        {
            GameLevelId = levelId;
        }

        private void Start()
        {
            UMUtils.Debug.Log($"Level Id: {GameLevelId}");
            UMini.UI.Open<GamePanel>();
            m_levelData = UMini.Config.GetTable<LevelTable>().GetDataById(GameLevelId);
            m_gameAudioData = UMini.Config.GetTable<GameAudioTable>().GetDataById(m_levelData.bgmId);
            UMini.Audio.BGM.Play(m_gameAudioData.path, m_gameAudioData.volume, m_gameAudioData.loop);
        }
    }
}