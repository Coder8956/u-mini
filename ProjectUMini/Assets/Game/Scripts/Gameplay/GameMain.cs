using System;
using Game.Scripts.UI.Game;
using UMiniFramework.Scripts;
using UMiniFramework.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class GameMain : MonoBehaviour
    {
        [SerializeField] private Camera m_gameCamera;
        [SerializeField] private GameObject m_cannon;
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

        void Update()
        {
            Vector3 mouseWorldPos = GetMousePosInWorld();
// 计算方向
            Vector3 direction = mouseWorldPos - transform.position;

            // 计算旋转
            Quaternion rotation = Quaternion.LookRotation(direction);
            
            // 设置旋转
            m_cannon.transform.rotation = rotation;
            if (Input.GetMouseButtonDown(0))
            {
            }
        }

        private Vector3 GetMousePosInWorld()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10; // 深度，具体值根据你的场景而定
            Vector3 worldPosition = m_gameCamera.ScreenToWorldPoint(mousePosition);
            return worldPosition;
        }
    }
}