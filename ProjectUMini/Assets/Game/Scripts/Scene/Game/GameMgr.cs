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
        [SerializeField] private Camera m_gameCamera;

        private LevelData m_levelData = null;

        private void Start()
        {
            // m_levelData = UMGR.Get<UMConfig>().GetTable<LevelTable>().GetDataById(GameGlobalVar.SelectLevelId);
            // UMGR.Get<UMAudio>().BGM.Play(m_levelData.bgmId);
            // GameUI.OpenGame();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
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

                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                // go.transform.position = mouseWorldPosition;
                go.AddComponent<Rigidbody>();
                // 计算发射方向
                Vector3 shootDirec = mouseWorldPosition - m_gameCamera.transform.position;
                
                // 在射线碰撞点实例化一个球体
                GameObject ball = Instantiate(go, m_gameCamera.transform.position, Quaternion.identity);

                // 获取球体的 Rigidbody 组件
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // 让球体沿射线方向发射
                    Vector3 direction = shootDirec;
                    rb.velocity = direction * 10; // 赋予球体一个初速度
                }
            }
        }
    }
}