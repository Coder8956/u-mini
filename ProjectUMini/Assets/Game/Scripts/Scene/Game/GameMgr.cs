using System;
using Game.Scripts.Common;
using Game.Scripts.Common.GameUI;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Config;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.Resource;
using UMiniFramework.Runtime.Modules.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Scene.Game
{
    public class GameMgr : MonoBehaviour
    {
        [SerializeField] private Camera m_gameCamera;

        private LevelData m_levelData = null;
        private BulletData m_bulletData = null;
        private BlockData m_blockData = null;

        private GameObject m_bulletPrefab = null;
        private GameObject m_blockPrefab = null;

        private float m_bulletInitSpeed = 5;

        private void Start()
        {
            m_levelData = UMGR.Get<UMConfig>().GetTable<LevelTable>().GetDataById(GameGlobalVar.SelectLevelId);
            m_bulletData = UMGR.Get<UMConfig>().GetTable<BulletTable>().GetDataById(m_levelData.bulletId);
            m_blockData = UMGR.Get<UMConfig>().GetTable<BlockTable>().GetDataById(m_levelData.blockId);

            m_bulletPrefab = UMGR.Get<UMRes>().Load<GameObject>(m_bulletData.bulletPath);
            m_blockPrefab = UMGR.Get<UMRes>().Load<GameObject>(m_blockData.blockPath);

            UMGR.Get<UMAudio>().BGM.Play(m_levelData.bgmId);
            GameUI.OpenGame();

            switch (m_levelData.id)
            {
                case "level_11001":
                    CreateLevelBlock_1();
                    break;
                case "level_11002":
                    CreateLevelBlock_2();
                    break;
                case "level_11003":
                    CreateLevelBlock_3();
                    break;
                default:
                    CreateLevelBlock_3();
                    break;
            }
        }

        private void CreateLevelBlock_1()
        {
            int row = 8;
            int column = 6;
            Vector3 blockPos = new Vector3(-row / 2 + 0.5f, 0, 0);
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < column; c++)
                {
                    blockPos.y = c;
                    Instantiate(m_blockPrefab, blockPos, Quaternion.identity);
                }

                blockPos.x += 1;
            }
        }

        private void CreateLevelBlock_2()
        {
            int row = 14;
            int column = 8;
            Vector3 blockPos = new Vector3(-row / 2 + 0.5f, 0, 4);
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < column; c++)
                {
                    blockPos.y = c;
                    Instantiate(m_blockPrefab, blockPos, Quaternion.identity);
                }

                blockPos.x += 1;
            }
        }

        private void CreateLevelBlock_3()
        {
            int row = 20;
            int column = 10;
            Vector3 blockPos = new Vector3(-row / 2 + 0.5f, 0, 7);
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < column; c++)
                {
                    blockPos.y = c;
                    Instantiate(m_blockPrefab, blockPos, Quaternion.identity);
                }

                blockPos.x += 1;
            }
        }

        private bool IsCanShoot()
        {
            return Input.GetMouseButtonDown(0)
                   && !UMGR.Get<UMUI>().IsClickUI();
        }

        private void Update()
        {
            if (IsCanShoot())
            {
                // Debug.Log("shoot");

                // 获取鼠标在屏幕上的位置
                Vector3 mouseScreenPosition = Input.mousePosition;

                // 将鼠标位置从屏幕坐标转换到世界坐标
                // z轴需要设置为一个合理的值，表示你希望鼠标指向的位置的深度
                // 比如，设置为相机到物体的距离
                mouseScreenPosition.z = 10f; // 假设你希望鼠标在离摄像机10个单位的位置

                // 获取世界坐标
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

                // 打印鼠标在3D空间中的位置
                // Debug.Log(mouseWorldPosition);

                // 计算发射方向
                Vector3 shootDirec = mouseWorldPosition - m_gameCamera.transform.position;

                // 在射线碰撞点实例化一个球体
                GameObject ball = Instantiate(m_bulletPrefab, m_gameCamera.transform.position, Quaternion.identity);

                ball.GetComponent<GameBullet>().SetData(m_bulletData);

                // 获取球体的 Rigidbody 组件
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // 让球体沿射线方向发射
                    Vector3 direction = shootDirec;
                    rb.velocity = direction * m_bulletInitSpeed; // 赋予球体一个初速度
                }
            }
        }
    }
}