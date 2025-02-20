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

        public List<AudioClipInfo> EffectClips;

        #endregion
    }
}