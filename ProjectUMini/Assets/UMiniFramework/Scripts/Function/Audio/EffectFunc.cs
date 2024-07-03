using UMiniFramework.Scripts.Kit;
using UMiniFramework.Scripts.Kit.Pool;
using UnityEngine;

namespace UMiniFramework.Scripts.Function.Audio
{
    public class EffectFunc : AudioFunc
    {
        private AudioSource m_originalEffect;
        private GameObjectPool m_effectPool;

        public override void Init()
        {
            m_originalEffect = CreateOriginalAudio();
            m_effectPool = CreateEffectAudioPool(m_originalEffect.gameObject, 5);
        }

        private GameObjectPool CreateEffectAudioPool(GameObject original, uint initCount)
        {
            GameObjectPool goPool = PoolTool.CreateGameObjectPool("AudioEffectPool", gameObject);
            goPool.Init(original, initCount);
            return goPool;
        }

        private AudioSource CreateOriginalAudio()
        {
            AudioSource origAudio = UMTool.CreateGameObject<AudioSource>(gameObject);
            origAudio.gameObject.name += "_0";
            return origAudio;
        }
    }
}