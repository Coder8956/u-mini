using UMiniFramework.Scripts.Pool;
using UMiniFramework.Scripts.Utils;
using UnityEngine;

namespace UMiniFramework.Scripts.Modules.AudioModule
{
    public class EffectAudio : BaseAudio
    {
        private GameObjectPool m_soundPool;

        public override void Init()
        {
            // 初始化音效对象池

            GameObject poolObjectTemplet = UMUtils.Tool.CreateGameObject<AudioSource>("Sound", gameObject).gameObject;
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