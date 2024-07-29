using System;
using UMiniFramework.Runtime.UMEntrance;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.AudioModule
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
                    UMUtilDebug.Warning($"Audio load failed. Path: {audioPath}");
                }
            });
        }
    }
}