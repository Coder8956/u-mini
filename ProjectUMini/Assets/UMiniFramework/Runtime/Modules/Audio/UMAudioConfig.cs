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

        private int m_defaultASCount = 0;

        /// <summary>
        /// 默认AudioSource数量.有效值 >=10;
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