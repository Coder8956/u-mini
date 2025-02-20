using UMiniFramework.Runtime.Common;

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
    }
}