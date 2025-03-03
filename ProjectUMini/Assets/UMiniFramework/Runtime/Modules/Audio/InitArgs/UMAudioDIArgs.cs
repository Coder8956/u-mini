using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.Audio.ClipInfo;
using UMiniFramework.Runtime.Modules.Audio.Effect;

namespace UMiniFramework.Runtime.Modules.Audio.InitArgs
{
    /// <summary>
    /// Audio模块 默认初始化参数
    /// </summary>
    public class UMAudioDIArgs
    {
        public static List<UMAudioClipInfo> BGMAudioClipInfoList()
        {
            return new();
        }

        public static List<UMAudioClipInfo> EffectAudioClipInfoList()
        {
            return new();
        }

        public static int EffectAudioDefaultAsCount()
        {
            return UMAudioEffect.MIN_AS_COUNT;
        }
    }
}