using UnityEngine;

namespace UMiniFramework.Runtime
{
    public static class UMLauncher
    {
        /// <summary>
        /// 是否已经启动
        /// </summary>
        public static bool IsWorked { get; private set; }

        private static GameObject m_root = null;

        /// <summary>
        /// 执行启动框架（全局仅执行一次）
        /// </summary>
        public static void Work()
        {
            if (IsWorked)
                return;

            IsWorked = true;

            m_root = new GameObject("UMini");
            GameObject.DontDestroyOnLoad(m_root);
            m_root.transform.position = Vector3.zero;

            // 创建框架模块
            UMConfig.Create(m_root);
            UMEvent.Create(m_root);
            UMLocal.Create(m_root);
            UMPersist.Create(m_root);
            UMRes.Create(m_root);
            UMScene.Create(m_root);
            UMUI.Create(m_root);

            // 后续继续添加
            // UMAudio.Create();
            // UMEvent.Create();
            // UMResource.Create();
        }
    }
}