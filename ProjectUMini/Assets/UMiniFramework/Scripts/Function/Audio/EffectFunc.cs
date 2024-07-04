using UMiniFramework.Scripts.Kit;
using UnityEngine;

namespace UMiniFramework.Scripts.Function.Audio
{
    public class EffectFunc : AudioFunc
    {
        private AudioSource m_originalEffect;

        public override void Init()
        {
            m_originalEffect = CreateOriginalAudio();
        }

        private AudioSource CreateOriginalAudio()
        {
            AudioSource origAudio = UMTool.CreateGameObject<AudioSource>(gameObject);
            origAudio.gameObject.name += "_0";
            return origAudio;
        }
    }
}