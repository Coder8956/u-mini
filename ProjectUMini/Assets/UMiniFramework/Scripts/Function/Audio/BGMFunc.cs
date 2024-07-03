using UnityEngine;

namespace UMiniFramework.Scripts.Function.Audio
{
    public class BGMFunc : AudioFunc
    {
        private AudioSource m_audio = null;

        public override void Init()
        {
            m_audio = gameObject.AddComponent<AudioSource>();
            m_audio.loop = true;
        }
    }
}