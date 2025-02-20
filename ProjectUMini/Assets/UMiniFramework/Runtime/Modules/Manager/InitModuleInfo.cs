using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.Manager
{
    public class InitModuleInfo
    {
        private bool m_initState = false;

        public bool InitState
        {
            get => m_initState;
            set => m_initState = value;
        }

        private UMBaseModule m_initModule = null;

        public UMBaseModule InitModule
        {
            get => m_initModule;
            set => m_initModule = value;
        }

        private float m_initProgress = 0;

        public float InitProgress
        {
            get => m_initProgress;
            set => m_initProgress = value;
        }
    }
}