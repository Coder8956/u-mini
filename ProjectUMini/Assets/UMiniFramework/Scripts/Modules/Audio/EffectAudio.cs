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
            // 初始化音效对象池

            GameObject poolObjectTemplet = UMTool.CreateGameObject<AudioSource>("Sound", gameObject).gameObject;
            GameObjectPool.PoolConfig poolConfig = new GameObjectPool.PoolConfig
            ("SoundPool",
                gameObject,
                poolObjectTemplet,
                5,
                null,
                null
            );
            m_soundPool = GameObjectPool.CreatePool(poolConfig);
            // m_originalEffect = CreateOriginalAudio();
        }


        // private AudioSource CreateOriginalAudio()
        // {
        //     AudioSource origAudio = UMTool.CreateGameObject<AudioSource>("Sound", gameObject);
        //     return origAudio;
        // }
    }
}