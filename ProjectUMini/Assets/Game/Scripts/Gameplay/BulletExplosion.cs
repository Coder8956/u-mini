using System.Collections;
using UMiniFramework.Scripts.UMEntrance;
using UMiniFramework.Scripts.Pool.GameObjectPool;
using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class BulletExplosion : MonoBehaviour
    {
        private ParticleSystem m_particleSystem;
        private UMGameObjectPool m_pool;
        private static GameAudioData m_ExplosionAudioData;

        public void Play(Vector3 worldPos, UMGameObjectPool pool)
        {
            if (m_ExplosionAudioData == null)
            {
                m_ExplosionAudioData = UMini.Config.GetTable<GameAudioTable>().GetDataById("effect_248013");
            }

            if (m_particleSystem == null)
            {
                m_particleSystem = GetComponent<ParticleSystem>();
            }

            m_pool = pool;
            transform.SetParent(null);
            transform.position = worldPos;
            m_particleSystem.Play();
            UMini.Audio.Effect.Play(m_ExplosionAudioData.path, m_ExplosionAudioData.volume);
            StartCoroutine(CheckIfParticleSystemIsComplete());
        }

        IEnumerator CheckIfParticleSystemIsComplete()
        {
            while (m_particleSystem.isPlaying)
            {
                yield return null;
            }

            OnParticleSystemComplete();
        }

        void OnParticleSystemComplete()
        {
            m_pool.Back(gameObject);
        }
    }
}