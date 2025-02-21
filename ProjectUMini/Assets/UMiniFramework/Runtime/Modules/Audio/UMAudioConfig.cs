using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.Audio
{
    /// <summary>
    /// UM 模块配置
    /// </summary>
    public class UMAudioConfig : UMModuleConfig
    {
        #region BGM-Config

        public List<AudioClipInfo> BGMClips;

        #endregion

        //=========================================

        #region Effect-Config

        private int m_defaultASCount = 5;

        /// <summary>
        /// 默认AudioSource数量.最小不能低于3;
        /// </summary>
        public int DefaultAsCount
        {
            get => m_defaultASCount;
            set => m_defaultASCount = value;
        }

        public List<AudioClipInfo> EffectClips;

        #endregion
    }
}