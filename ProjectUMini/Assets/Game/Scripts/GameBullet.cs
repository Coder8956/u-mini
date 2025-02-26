using System;
using Game.Scripts.Common;
using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class GameBullet : MonoBehaviour
    {
        private BulletData m_data;
        private Rigidbody m_rb;
        public UnityAction<GameObject> OnDestroyBlock { get; set; }

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
            if (m_rb != null)
            {
                // 沿射线方向发射
                m_rb.velocity = direction * speed; // 赋予一个初速度
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            UMGR.Get<UMAudio>().Effect.Play(m_data.exploAudioId);
            Instantiate(GameGlobalVar.VFX_Explosion, transform.position, Quaternion.identity);
            // m_rb.AddExplosionForce(10, transform.position, 10);

            if (other.gameObject.CompareTag("Monster"))
            {
                OnDestroyBlock?.Invoke(other.gameObject);
                Destroy(other.gameObject);
            }

            Destroy(gameObject);
        }
    }
}