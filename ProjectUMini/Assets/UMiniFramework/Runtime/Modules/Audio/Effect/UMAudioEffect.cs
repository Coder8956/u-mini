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

        private int m_defaultASCount = 5;

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

        /// <summary>
        /// 初始化 AudioSource 队列
        /// </summary>
        private void InitASQue()
        {
            m_asQue = new Queue<AudioSource>();
            for (int i = 0; i < m_defaultASCount; i++)
            {
                m_asQue.Enqueue(CreateAS());
            }
        }

        private AudioSource CreateAS()
        {
            AudioSource new_as = gameObject.AddComponent<AudioSource>();
            new_as.playOnAwake = false;
            new_as.enabled = false;
            return new_as;
        }

        private void ReadConfig(UMAudioConfig config)
        {
            m_config = config;
            if (m_config == null) return;
            m_defaultASCount = Mathf.Clamp(m_config.DefaultAsCount, 5, int.MaxValue);
        }

        private IEnumerator WaitEffectPlayOver(AudioSource audioSource)
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            audioSource.clip = null;
            audioSource.enabled = false;
            m_asQue.Enqueue(audioSource);
        }

        protected override void Init(UMAudioConfig config)
        {
            ReadConfig(config);
            InitEffectClipDic();
            InitASQue();
        }


        public void Play(string id, float volume = 1)
        {
            AudioClipInfo aci = m_EffectClipDic[id];

            if (aci.Clip == null)
            {
                LoadClipInACI(aci);
            }

            AudioSource curtAS = null;

            if (m_asQue.Count == 0)
            {
                curtAS = CreateAS();
            }
            else
            {
                curtAS = m_asQue.Dequeue();
            }

            curtAS.clip = aci.Clip;
            curtAS.volume = volume;
            curtAS.Play();

            StartCoroutine(WaitEffectPlayOver(curtAS));
        }
    }
}