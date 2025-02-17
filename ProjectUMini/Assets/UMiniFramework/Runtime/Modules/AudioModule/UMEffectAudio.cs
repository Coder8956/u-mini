﻿using System.Collections;
using System.Collections.Generic;
using UMiniFramework.Runtime.Pool.GameObjectPool;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.AudioModule
{
    public class UMEffectAudio : UMAudio
    {
        private UMGameObjectPool m_soundPool;
        private Dictionary<string, AudioClip> m_cachedAudioClipDic;
        private List<string> m_loadingClip;
        private bool m_isMute = false;

        public override void Init()
        {
            // 初始化音效对象池
            GameObject poolObjectTemplet = UMUtilCommon.CreateGameObject<AudioSource>("Sound", gameObject).gameObject;
            UMGameObjectPool.UMPoolConfig poolConfig = new UMGameObjectPool.UMPoolConfig
            ("SoundPool",
                gameObject,
                poolObjectTemplet,
                5,
                null,
                null
            );
            m_soundPool = UMGameObjectPool.CreatePool(poolConfig);
            m_cachedAudioClipDic = new Dictionary<string, AudioClip>();
            m_loadingClip = new List<string>();
        }

        public void Play(string audioPath, float volume = 1)
        {
            AudioClip effectAC = null;
            if (m_loadingClip.Contains(audioPath)) return;
            if (m_cachedAudioClipDic.ContainsKey(audioPath))
            {
                effectAC = m_cachedAudioClipDic[audioPath];
                PlayEffect(effectAC, volume);
            }
            else
            {
                m_loadingClip.Add(audioPath);
                LoadAudioClip(audioPath, (clip) =>
                {
                    effectAC = clip;
                    m_cachedAudioClipDic.Add(audioPath, effectAC);
                    m_loadingClip.Remove(audioPath);
                    PlayEffect(effectAC, volume);
                });
            }
        }

        public void SetMute(bool val)
        {
            m_isMute = val;
        }

        public bool GetMute()
        {
            return m_isMute;
        }

        private void PlayEffect(AudioClip ac, float volume)
        {
            GameObject audioEffect = m_soundPool.Get();
            audioEffect.transform.SetParent(transform, false);
            AudioSource effectAS = audioEffect.GetComponent<AudioSource>();
            effectAS.clip = ac;
            effectAS.volume = volume;
            effectAS.mute = m_isMute;
            effectAS.Play();
            StartCoroutine(WaitEffectPlayOver(effectAS));
        }

        private IEnumerator WaitEffectPlayOver(AudioSource audioSource)
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            m_soundPool.Back(audioSource.gameObject);
        }
    }
}