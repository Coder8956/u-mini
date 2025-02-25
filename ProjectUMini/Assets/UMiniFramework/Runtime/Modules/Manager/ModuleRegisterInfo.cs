using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.Manager
{
    /// <summary>
    /// 用于记录功能模块的注册信息
    /// </summary>
    public class ModuleRegisterInfo
    {
        public ModuleRegisterInfo(UMBaseModule module, UMModuleInitArgs initArgs)
        {
            m_module = module;
            m_initArgs = initArgs;
        }

        private UMBaseModule m_module;

        public UMBaseModule Module => m_module;

        private UMModuleInitArgs m_initArgs;

        public UMModuleInitArgs InitArgs => m_initArgs;
    }
}