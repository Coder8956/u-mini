using System.Collections.Generic;
using UMiniFramework.Runtime.Common;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules
{
    public class UMAudioBGM : UMAudioFunc
    {
        private const string INVALID_BGM_ID = "UM_INVALID_BGM_ID";
        private AudioSource m_audioSource = null;
        private Dictionary<string, UMAudioClipInfo> m_BGMClipDic;

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

        private void InitAudioBGM()
        {
            CurtBGMID = INVALID_BGM_ID;
            m_BGMClipDic = new Dictionary<string, UMAudioClipInfo>();
            m_audioSource = gameObject.AddComponent<AudioSource>();
            m_audioSource.playOnAwake = false;
            m_audioSource.loop = true;
        }

        public void AddAudioClip(string id, string path, bool isPreLoad = false,
            UMResLoadType pathType = UMResLoadType.Resources)
        {
            AddAudioClip(new UMAudioClipInfo(id, path, isPreLoad, pathType));
        }

        public void AddAudioClip(UMAudioClipInfo aci)
        {
            m_BGMClipDic.Add(aci.ID, aci);
            if (aci.IsPreLoad)
            {
                LoadClipInACI(aci);
            }
        }

        public void Play(string id, bool loop = true)
        {
            CurtBGMID = id;
            UMAudioClipInfo aci = m_BGMClipDic[CurtBGMID];

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