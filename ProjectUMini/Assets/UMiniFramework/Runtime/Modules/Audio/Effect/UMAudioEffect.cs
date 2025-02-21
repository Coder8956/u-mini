using System.Collections;
using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.Audio.Base;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Audio.Effect
{
    public class UMAudioEffect : UMAudioFunc
    {
        private Dictionary<string, AudioClipInfo> m_EffectClipDic;
        private UMAudioConfig m_config;
        private Queue<AudioSource> m_asQue;

        private const int MIN_AS_COUNT = 3;
        private int m_initASCount = 0;
        private int m_createdASCount = 0;

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
            if (m_config != null)
            {
                m_initASCount = Mathf.Clamp(m_config.DefaultAsCount, MIN_AS_COUNT, int.MaxValue);
            }

            m_asQue = new Queue<AudioSource>();
            for (int i = 0; i < m_initASCount; i++)
            {
                m_asQue.Enqueue(CreateAS());
            }
        }

        private AudioSource GetAS()
        {
            AudioSource getAS = null;

            if (m_asQue.Count == 0)
            {
                getAS = CreateAS();
            }
            else
            {
                getAS = m_asQue.Dequeue();
            }

            getAS.enabled = true;

            return getAS;
        }

        private void BackAS(AudioSource backAS)
        {
            backAS.clip = null;
            backAS.enabled = false;
            m_asQue.Enqueue(backAS);
        }

        private AudioSource CreateAS()
        {
            AudioSource new_as = gameObject.AddComponent<AudioSource>();
            new_as.playOnAwake = false;
            new_as.enabled = false;
            m_createdASCount++;
            return new_as;
        }

        private IEnumerator WaitEffectPlayOver(AudioSource audioSource)
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            BackAS(audioSource);
        }

        protected override void Init(UMAudioConfig config)
        {
            m_config = config;
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

            AudioSource curtAS = GetAS();
            curtAS.clip = aci.Clip;
            curtAS.volume = volume;
            curtAS.Play();

            StartCoroutine(WaitEffectPlayOver(curtAS));
        }

        public void PrintASCount()
        {
            UMUtilDebug.Log($"Created AS Count: {m_createdASCount}");
            UMUtilDebug.Log($"AS  Count: {m_createdASCount}");
        }
    }
}