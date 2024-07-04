using UnityEngine;

namespace UMiniFramework.Scripts.Modules.Audio
{
    public class BGMAudio : AudioFunc
    {
        private AudioSource m_audio = null;

        public override void Init()
        {
            m_audio = gameObject.AddComponent<AudioSource>();
            m_audio.loop = true;
        }
    }
}