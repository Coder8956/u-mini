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

        private List<AudioSource> m_asPlayingList;

        private const int MIN_AS_COUNT = 10;
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

        private bool m_mute = false;
        private float m_volume = 1;

        /// <summary>
        /// 静音属性
        /// </summary>
        public bool Mute
        {
            get { return m_mute; }
            set
            {
                m_mute = value;
                for (var i = 0; i < m_asPlayingList.Count; i++)
                {
                    m_asPlayingList[i].mute = m_mute;
                }
            }
        }

        /// <summary>
        /// 音量属性
        /// </summary>
        public float Volume
        {
            get { return m_volume; }
            set
            {
                m_volume = value;
                for (var i = 0; i < m_asPlayingList.Count; i++)
                {
                    m_asPlayingList[i].volume = m_volume;
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
            m_asPlayingList = new List<AudioSource>();

            for (int i = 0; i < m_initASCount; i++)
            {
                BackAS(CreateAS());
            }
        }

        /// <summary>
        /// 从 AudioSource 队列中获取一个对象
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 将 AudioSource 对象放回队列
        /// </summary>
        /// <returns></returns>
        private void BackAS(AudioSource backAS)
        {
            backAS.clip = null;
            backAS.enabled = false;

            if (m_asQue.Count >= m_initASCount)
            {
                Destroy(backAS);
            }
            else
            {
                m_asQue.Enqueue(backAS);
            }
        }

        /// <summary>
        /// 创建一个 AudioSource 对象
        /// </summary>
        /// <returns></returns>
        private AudioSource CreateAS()
        {
            AudioSource new_as = gameObject.AddComponent<AudioSource>();
            new_as.playOnAwake = false;
            new_as.enabled = false;
            m_createdASCount++;
            return new_as;
        }

        /// <summary>
        /// 等待音频播放完成
        /// </summary>
        /// <param name="audioSource"></param>
        /// <returns></returns>
        private IEnumerator WaitEffectPlayOver(AudioSource audioSource)
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            m_asPlayingList.Remove(audioSource);
            BackAS(audioSource);
        }

        protected override void Init(UMAudioConfig config)
        {
            m_config = config;
            InitEffectClipDic();
            InitASQue();
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="id"></param>
        /// <param name="volume"></param>
        public void Play(string id)
        {
            AudioClipInfo aci = m_EffectClipDic[id];

            if (aci.Clip == null)
            {
                LoadClipInACI(aci);
            }

            AudioSource curtAS = GetAS();
            m_asPlayingList.Add(curtAS);

            curtAS.clip = aci.Clip;
            curtAS.volume = m_volume;
            curtAS.mute = m_mute;
            curtAS.Play();

            StartCoroutine(WaitEffectPlayOver(curtAS));
        }

        /// <summary>
        /// 输出 AudioSource 相关信息
        /// </summary>
        public void PrintASInfo()
        {
            UMUtilDebug.Log(
                $"=== Print AS Info ===\n" +
                $"Created AS Count: {m_createdASCount}\n" +
                $"AS Queue Count: {m_asQue.Count}\n" +
                $"AS Playing List Count: {m_asPlayingList.Count}\n" +
                $"=====================");
        }
    }
}