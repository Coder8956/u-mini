using System.Collections;
using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.Audio.Base;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Audio.Effect
{
    public class UMAudioEffect : UMAudioFunc
    {
        private Dictionary<string, AudioClipInfo> m_EffectClipDic;
        private UMAudioConfig m_config;
        private Queue<AudioSource> m_asQue;

        /// <summary>
        /// 初始化 Effect Clip 字典
        /// </summary>
        private void InitEffectClipDic()
        {
            m_EffectClipDic = new Dictionary<string, AudioClipInfo>();
            if (m_config == null) return;
            if (m_config.EffectClips == null) return;
            AudioClipInfo aci = null;
            for (var i = 0; i < m_config.EffectClips.Count; i++)
            {
                aci = m_config.EffectClips[i];
                m_EffectClipDic.Add(aci.ID, aci);
                if (aci.IsPreLoad)
                {
                    LoadClipInACI(aci);
                }
            }
        }

        protected override void Init(UMAudioConfig config)
        {
            m_config = config;
            InitEffectClipDic();

            m_asQue = new Queue<AudioSource>();
            
            // TODO: 开始处理 as 队列
        }

        public void Play(string audioPath, float volume = 1)
        {
        }

        public void SetMute(bool val)
        {
            // m_isMute = val;
        }

        public bool GetMute()
        {
            return false; //m_isMute;
        }

        private void PlayEffect(AudioClip ac, float volume)
        {
            // GameObject audioEffect = m_soundPool.Get();
            // audioEffect.transform.SetParent(transform, false);
            // AudioSource effectAS = audioEffect.GetComponent<AudioSource>();
            // effectAS.clip = ac;
            // effectAS.volume = volume;
            // effectAS.mute = m_isMute;
            // effectAS.Play();
            // StartCoroutine(WaitEffectPlayOver(effectAS));
        }

        private IEnumerator WaitEffectPlayOver(AudioSource audioSource)
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            // m_soundPool.Back(audioSource.gameObject);
        }
    }
}