using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.Manager
{
    public class InitProgressInfo
    {
        /// <summary>
        /// 标记初始化状态,初始化完成为 true
        /// </summary>
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