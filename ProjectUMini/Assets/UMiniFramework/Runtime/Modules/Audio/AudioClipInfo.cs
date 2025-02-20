namespace UMiniFramework.Runtime.Modules.Audio
{
    /// <summary>
    /// 音频信息
    /// </summary>
    public class AudioClipInfo
    {
        public readonly string Path;
        public bool IsPreLoad;

        public AudioClipInfo(string path, bool isPreLoad)
        {
            Path = path;
            IsPreLoad = isPreLoad;
        }
    }
}