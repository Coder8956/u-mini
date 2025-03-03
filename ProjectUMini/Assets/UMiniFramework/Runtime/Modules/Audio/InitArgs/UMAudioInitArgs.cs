using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.Audio.ClipInfo;
using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.Audio.InitArgs
{
    /// <summary>
    /// Audio模块 初始化参数
    /// </summary>
    public class UMAudioInitArgs : UMModuleInitArgs
    {
        public UMAudioInitArgs()
        {
            BGMClips = UMAudioDIArgs.BGMAudioClipInfoList();

            EffectClips = UMAudioDIArgs.EffectAudioClipInfoList();
            m_defaultASCount = UMAudioDIArgs.EffectAudioDefaultAsCount();
        }

        #region BGM-Config

        public List<UMAudioClipInfo> BGMClips;

        #endregion

        //=========================================

        #region Effect-Config

        private int m_defaultASCount = 0;

        /// <summary>
        /// 默认AudioSource数量.有效值 >=3;
        /// </summary>
        public int DefaultAsCount
        {
            get => m_defaultASCount;
            set => m_defaultASCount = value;
        }

        public List<UMAudioClipInfo> EffectClips;

        #endregion
    }
}