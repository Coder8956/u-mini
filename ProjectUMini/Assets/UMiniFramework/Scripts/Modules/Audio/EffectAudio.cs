using UMiniFramework.Scripts.Kit;
using UMiniFramework.Scripts.Pool;
using UnityEngine;

namespace UMiniFramework.Scripts.Modules.Audio
{
    public class EffectAudio : BaseAudio
    {
        private GameObjectPool m_soundPool;

        public override void Init()
        {
            m_soundPool = GameObjectPool.CreatePool("SoundPool", gameObject);
            // m_originalEffect = CreateOriginalAudio();
        }

        // private AudioSource CreateOriginalAudio()
        // {
        //     AudioSource origAudio = UMTool.CreateGameObject<AudioSource>("Sound", gameObject);
        //     return origAudio;
        // }
    }
}