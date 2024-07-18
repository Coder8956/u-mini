using System;
using Game.Scripts.Gameplay.MonsterCreator;
using Game.Scripts.UI.Game;
using UMiniFramework.Scripts;
using UMiniFramework.Scripts.Pool.GameObjectPool;
using UMiniFramework.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class GameMain : MonoBehaviour
    {
        [SerializeField] private Camera m_gameCamera;
        [SerializeField] private GameObject m_bulletExplosion;
        [SerializeField] private GameObject m_cannonPlace;
        [SerializeField] private MonsterCreatorBase[] m_monsterCreators;
        private GameCannon m_gameCannon;
        private Vector3 cannonLookPos, gunLookPos;
        private static string GameLevelId = string.Empty;

        private LevelData m_levelData;
        private GameAudioData m_bgmAudioData;
        private GameAudioData m_gunSoundData;
        private CannonData m_cannonData;
        private BulletData m_bulletData;

        private GameObjectPool m_bulletPool;
        private GameObjectPool m_bulletExplosionPool;

        public static void SetGameLevel(string levelId)
        {
            GameLevelId = levelId;
        }

        private void Start()
        {
            UMUtils.Debug.Log($"Level Id: {GameLevelId}");
            UMini.UI.Open<GamePanel>();

            m_levelData = UMini.Config.GetTable<LevelTable>().GetDataById(GameLevelId);
            m_cannonData = UMini.Config.GetTable<CannonTable>().GetDataById(m_levelData.cannonId);
            m_bgmAudioData = UMini.Config.GetTable<GameAudioTable>().GetDataById(m_levelData.bgmId);
            m_gunSoundData = UMini.Config.GetTable<GameAudioTable>().GetDataById(m_cannonData.gunSound);
            m_bulletData = UMini.Config.GetTable<BulletTable>().GetDataById(m_cannonData.bulletId);

            UMini.Audio.BGM.Play(m_bgmAudioData.path, m_bgmAudioData.volume, m_bgmAudioData.loop);

            // 加载大炮
            UMini.Asset.LoadAsync<GameObject>(m_cannonData.cannonPath, (res) =>
            {
                m_gameCannon = Instantiate(res.Resource).GetComponent<GameCannon>();
                m_gameCannon.transform.position = m_cannonPlace.transform.position;
            });

            // 加载炮弹预制体
            UMini.Asset.LoadAsync<GameObject>(m_bulletData.bulletPath, (res) =>
            {
                GameObject bullet = Instantiate(res.Resource);
                m_bulletPool = GameObjectPool.CreatePool(new GameObjectPool.PoolConfig(
                    "BulletPool",
                    null,
                    bullet,
                    10,
                    null,
                    null
                ));
            });

            // 子弹爆炸特效
            m_bulletExplosionPool = GameObjectPool.CreatePool(new GameObjectPool.PoolConfig(
                "BulletExplosionPool",
                null,
                Instantiate(m_bulletExplosion),
                10,
                null,
                null
            ));
        }

        void Update()
        {
            RotateCannon();
            CannonFire();
        }

        private void RotateCannon()
        {
            if (m_gameCannon == null) return;
            Vector3 mouseWorldPos = GetMousePosInWorld();
            cannonLookPos = gunLookPos = mouseWorldPos;

            // 调整炮台的位置
            cannonLookPos.y = m_cannonPlace.transform.position.y;
            m_gameCannon.CannonDeck.transform.LookAt(cannonLookPos);
            // Debug.DrawLine(m_gameCannon.CannonDeck.transform.position, cannonLookPos);

            // 调整炮管的位置
            // Debug.Log(gunLookPos.y);
            gunLookPos.y = Mathf.Clamp(gunLookPos.y, 2.3f, 3.4f);
            m_gameCannon.Gun.transform.LookAt(gunLookPos);
            // Debug.DrawLine(m_gameCannon.Gun.transform.position, gunLookPos);
        }

        private void CannonFire()
        {
            if (m_gameCannon == null) return;
            if (Input.GetMouseButtonDown(0))
            {
                UMini.Audio.Effect.Play(m_gunSoundData.path);
                m_gameCannon.FireParticle.Play();
                GameObject bulletGO = m_bulletPool.Get();
                GameBullet bullet = bulletGO.GetComponent<GameBullet>();
                bullet.Shooting(m_gameCannon.ShootingPoint, 500, m_bulletPool, m_bulletExplosionPool);
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