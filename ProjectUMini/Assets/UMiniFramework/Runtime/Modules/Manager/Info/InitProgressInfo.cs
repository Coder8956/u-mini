using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.Manager.Info
{
    public class InitProgressInfo
    {
        private bool m_initState = false;

        /// <summary>
        /// 标记初始化状态,初始化完成为 true
        /// </summary>
        public bool InitState
        {
            get => m_initState;
        }

        private UMBaseModule m_initModule = null;

        public UMBaseModule InitModule
        {
            get => m_initModule;
        }

        private float m_initProgress = 0;

        public float InitProgress
        {
            get => m_initProgress;
        }
    }
}