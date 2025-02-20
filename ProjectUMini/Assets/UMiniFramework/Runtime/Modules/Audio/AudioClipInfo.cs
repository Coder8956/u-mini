using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Audio
{
    /// <summary>
    /// 音频信息
    /// </summary>
    public class AudioClipInfo
    {
        public readonly string ID;
        public readonly string Path;
        public readonly bool IsPreLoad;
        public readonly UMResPathType PathType;

        public AudioClip Clip { get; private set; }

        public AudioClipInfo(
            string id,
            string path,
            bool isPreLoad = false,
            UMResPathType pathType = UMResPathType.Resources)
        {
            ID = id;
            Path = path;
            IsPreLoad = isPreLoad;
            PathType = pathType;
        }

        private void LoadClip()
        {
            if (PathType == UMResPathType.Resources)
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