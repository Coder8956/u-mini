using System;
using UnityEngine;

namespace Game.Scripts
{
    public class VFXExplosion : MonoBehaviour
    {
        private ParticleSystem m_particleSystem;

        private void Awake()
        {
            m_particleSystem = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (!m_particleSystem.isPlaying)
            {
                Destroy(gameObject);
                // 粒子播放完成
                // Debug.Log("Particle system has finished playing!");
            }
        }
    }
}