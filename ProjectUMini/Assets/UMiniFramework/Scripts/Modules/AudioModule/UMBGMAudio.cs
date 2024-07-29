using System.Collections.Generic;
using UnityEngine;

namespace UMiniFramework.Scripts.Modules.AudioModule
{
    public class UMBGMAudio : UMAudio
    {
        private AudioSource m_audioSource = null;

        private Dictionary<string, AudioClip> m_cachedAudioClipDic;

        public override void Init()
        {
            m_audioSource = gameObject.AddComponent<AudioSource>();
            m_audioSource.loop = true;
            m_cachedAudioClipDic = new Dictionary<string, AudioClip>();
        }

        public void Play(string audioPath, float volume = 1, bool loop = true)
        {
            AudioClip bgmAC = null;
            if (m_cachedAudioClipDic.ContainsKey(audioPath))
            {
                bgmAC = m_cachedAudioClipDic[audioPath];
                SwitchClip(bgmAC, volume, loop);
            }
            else
            {
                LoadAudioClip(audioPath, (clip) =>
                {
                    bgmAC = clip;
                    m_cachedAudioClipDic.Add(audioPath, bgmAC);
                    SwitchClip(bgmAC, volume, loop);
                });
            }
        }

        public void Stop()
        {
            if (m_audioSource == null) return;
            m_audioSource.Stop();
        }

        public void SetMute(bool val)
        {
            m_audioSource.mute = val;
        }

        public bool GetMute()
        {
            return m_audioSource.mute;
        }

        private void SwitchClip(AudioClip clip, float volume, bool loop)
        {
            m_audioSource.clip = clip;
            m_audioSource.volume = volume;
            m_audioSource.loop = loop;
            m_audioSource.Play();
        }
    }
}