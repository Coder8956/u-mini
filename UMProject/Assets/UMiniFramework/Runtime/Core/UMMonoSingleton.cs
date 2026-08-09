using UnityEngine;

namespace UMiniFramework.Runtime
{
    /// <summary>
    /// MonoBehaviour 单例
    /// 只能由框架创建
    /// </summary>
    public abstract class UMMonoSingleton<T> : MonoBehaviour
        where T : UMMonoSingleton<T>
    {
        private static T m_Instance;
        private static bool m_IsCreating;

        /// <summary>
        /// 子类使用，外部不可见
        /// </summary>
        protected static T Instance => m_Instance;

        public static bool IsCreated => m_Instance != null;

        /// <summary>
        /// 创建单例
        /// </summary>
        internal static T Create()
        {
            if (m_Instance != null)
                return m_Instance;

            m_IsCreating = true;

            var go = new GameObject(typeof(T).Name);

            DontDestroyOnLoad(go);

            go.AddComponent<T>();

            m_IsCreating = false;

            return m_Instance;
        }

        internal static T Create(GameObject parent)
        {
            Create();
            m_Instance.transform.SetParent(parent.transform);
            return m_Instance;
        }

        protected virtual void Awake()
        {
            if (!m_IsCreating)
            {
                Debug.LogError($"{typeof(T).Name} 只能通过 Create() 创建。");
                DestroyImmediate(gameObject);
                return;
            }

            if (m_Instance != null && m_Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            m_Instance = (T) this;

            OnInit();
        }

        protected abstract void OnInit();

        protected virtual void OnDestroy()
        {
            if (m_Instance == this)
            {
                m_Instance = null;
            }
        }
    }
}