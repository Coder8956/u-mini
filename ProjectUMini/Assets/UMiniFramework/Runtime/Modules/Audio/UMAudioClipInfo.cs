using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Audio
{
    /// <summary>
    /// 音频信息
    /// </summary>
    public class UMAudioClipInfo
    {
        public readonly string ID;
        public readonly string Path;
        public readonly bool IsPreLoad;
        public readonly UMResLoadType PathType;

        public AudioClip Clip { get; private set; }

        public UMAudioClipInfo(
            string id,
            string path,
            bool isPreLoad = false,
            UMResLoadType pathType = UMResLoadType.Resources)
        {
            ID = id;
            Path = path;
            IsPreLoad = isPreLoad;
            PathType = pathType;
        }

        private void LoadClip()
        {
            if (PathType == UMResLoadType.Resources)
            {
                Clip = Resources.Load<AudioClip>(Path);
            }
            else
            {
                UMUtilDebug.Warning($"Invalid parameter: {PathType}");
            }
        }
    }
}