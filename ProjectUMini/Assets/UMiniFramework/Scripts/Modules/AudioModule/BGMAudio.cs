using UnityEngine;

namespace UMiniFramework.Scripts.Modules.AudioModule
{
    public class BGMAudio : BaseAudio
    {
        private AudioSource m_audio = null;

        public override void Init()
        {
            m_audio = gameObject.AddComponent<AudioSource>();
            m_audio.loop = true;
        }
    }
}