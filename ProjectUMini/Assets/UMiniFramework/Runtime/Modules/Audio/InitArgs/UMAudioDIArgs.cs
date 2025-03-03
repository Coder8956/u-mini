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

        public static int EffectAudioDefaultEASCount()
        {
            return UMAudioEffect.MIN_EAS_COUNT;
        }
    }
}