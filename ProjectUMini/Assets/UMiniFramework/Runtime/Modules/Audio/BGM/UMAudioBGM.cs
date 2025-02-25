using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.Audio.Base;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Audio.BGM
{
    public class UMAudioBGM : UMAudioFunc
    {
        private const string INVALID_BGM_ID = "UM_INVALID_BGM_ID";
        private AudioSource m_audioSource = null;
        private UMAudioConfig m_config;
        private Dictionary<string, AudioClipInfo> m_BGMClipDic;

        /// <summary>
        /// 静音属性
        /// </summary>
        public bool Mute
        {
            get { return m_audioSource.mute; }
            set { m_audioSource.mute = value; }
        }

        /// <summary>
        /// 音量属性
        /// </summary>
        public float Volume
        {
            get { return m_audioSource.volume; }
            set { m_audioSource.volume = value; }
        }

        public string CurtBGMID { get; private set; }

        /// <summary>
        /// 初始化 BGM Clip 字典
        /// </summary>
        private void InitBGMClipDic()
        {
            m_BGMClipDic = new Dictionary<string, AudioClipInfo>();
            if (m_config == null) return;
            if (m_config.BGMClips == null) return;
            AudioClipInfo aci = null;
            for (var i = 0; i < m_config.BGMClips.Count; i++)
            {
                aci = m_config.BGMClips[i];
                m_BGMClipDic.Add(aci.ID, aci);
                if (aci.IsPreLoad)
                {
                    LoadClipInACI(aci);
                }
            }
        }

        protected override void Init(UMAudioConfig config)
        {
            CurtBGMID = INVALID_BGM_ID;

            m_config = config;

            InitBGMClipDic();

            m_audioSource = gameObject.AddComponent<AudioSource>();
            m_audioSource.playOnAwake = false;
            m_audioSource.loop = true;
        }

        public void Play(string id, bool loop = true)
        {
            CurtBGMID = id;
            AudioClipInfo aci = m_BGMClipDic[CurtBGMID];

            if (aci.Clip == null)
            {
                LoadClipInACI(aci);
            }

            m_audioSource.clip = aci.Clip;
            m_audioSource.loop = loop;
            m_audioSource.Play();
        }

        public void Stop()
        {
            if (m_audioSource == null) return;
            m_audioSource.Stop();
        }
    }
}