using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.Manager
{
    /// <summary>
    /// 用于记录功能模块的注册信息
    /// </summary>
    public class ModuleRegisterInfo
    {
        public ModuleRegisterInfo(UMBaseModule module, UMModuleConfig config)
        {
            m_module = module;
            m_config = config;
        }

        private UMBaseModule m_module;

        public UMBaseModule Module => m_module;

        private UMModuleConfig m_config;

        public UMModuleConfig Config => m_config;
    }
}