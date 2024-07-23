using System;
using UMiniFramework.Scripts.UMEntrance;
using UMiniFramework.Scripts.Utils;
using UnityEngine;

namespace UMiniFramework.Scripts.Modules.AudioModule
{
    public abstract class UMAudio : MonoBehaviour
    {
        public abstract void Init();

        protected void LoadAudioClip(string audioPath, Action<AudioClip> onCompleted)
        {
            UMini.Asset.LoadAsync<AudioClip>(audioPath, (res) =>
            {
                if (res.State)
                {
                    onCompleted?.Invoke(res.Resource);
                }
                else
                {
                    UMUtils.Debug.Warning($"Audio load failed. Path: {audioPath}");
                }
            });
        }
    }
}