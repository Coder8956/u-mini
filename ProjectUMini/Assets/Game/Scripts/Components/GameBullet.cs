using System;
using Game.Scripts.Common;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Components
{
    public class GameBullet : MonoBehaviour
    {
        private BulletData m_data;
        private Rigidbody m_rb;
        public UnityAction<GameObject> OnDestroyBlock { get; set; }
        public UnityAction<GameObject> OnHitGo { get; set; }

        private GameObject HitGO = null;

        private void Awake()
        {
            // Rigidbody 组件
            m_rb = GetComponent<Rigidbody>();
        }

        public void SetData(BulletData data)
        {
            m_data = data;
        }

        public void Shoot(Vector3 direction, float speed)
        {
            HitGO = null;
            if (m_rb != null)
            {
                // 沿射线方向发射
                m_rb.velocity = direction * speed; // 赋予一个初速度
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (HitGO != null) return;

            UMGR.Get<UMAudio>().Effect.Play(m_data.exploAudioId);
            // m_rb.AddExplosionForce(10, transform.position, 10);

            if (other.gameObject.CompareTag("Monster"))
            {
                HitGO = other.gameObject;
                OnDestroyBlock?.Invoke(HitGO);
                Destroy(HitGO);
            }

            OnHitGo?.Invoke(gameObject);
            OnHitGo = null;
        }
    }
}