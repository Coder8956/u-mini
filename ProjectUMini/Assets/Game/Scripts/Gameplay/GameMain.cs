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
        [SerializeField] private GameObject m_gun;
        private Vector3 cannonLookPos, gunLookPos;
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
            cannonLookPos = gunLookPos = mouseWorldPos;

            // 调整炮台的位置
            cannonLookPos.y = m_cannon.transform.position.y;
            m_cannon.transform.LookAt(cannonLookPos);
            Debug.DrawLine(m_cannon.transform.position, cannonLookPos);

            // 调整炮管的位置
            gunLookPos.y = Mathf.Clamp(gunLookPos.y, 2.3f, float.MaxValue);
            m_gun.transform.LookAt(gunLookPos);
            Debug.DrawLine(m_gun.transform.position, gunLookPos);
            if (Input.GetMouseButtonDown(0))
            {
            }
        }

        private Vector3 GetMousePosInWorld()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 3; // 深度，具体值根据你的场景而定
            Vector3 worldPosition = m_gameCamera.ScreenToWorldPoint(mousePosition);
            return worldPosition;
        }
    }
}