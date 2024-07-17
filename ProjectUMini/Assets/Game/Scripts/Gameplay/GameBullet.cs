using System;
using UMiniFramework.Scripts.Pool.GameObjectPool;
using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class GameBullet : MonoBehaviour
    {
        [SerializeField] private GameObject m_bulletBody;
        [SerializeField] private ParticleSystem m_bulletParticle;
        private Rigidbody m_bulletRig;
        public Rigidbody BulletRig => m_bulletRig;
        private GameObjectPool m_bulletPool;

        private void Awake()
        {
            m_bulletRig = GetComponent<Rigidbody>();
        }

        private void DisableRigidbody()
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        public void SetPoolReference(GameObjectPool pool)
        {
            if (m_bulletPool == null)
            {
                m_bulletPool = pool;
            }
        }

        public void PrepareShoot()
        {
            m_bulletBody.SetActive(true);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        private void OnCollisionEnter(Collision other)
        {
            DisableRigidbody();
            m_bulletParticle.gameObject.SetActive(true);
            m_bulletParticle.Play();
            m_bulletBody.gameObject.SetActive(false);
            Invoke(nameof(BackPool), 2f);
        }

        private void BackPool()
        {
            m_bulletParticle.Stop(); // 首先停止粒子系统  
            m_bulletParticle.gameObject.SetActive(false);
            m_bulletPool.Back(gameObject);
        }
    }
}