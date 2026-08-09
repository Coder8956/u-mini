using UnityEngine;

namespace UMiniFramework.Runtime
{
    public abstract class UMLocalComponent : MonoBehaviour
    {
        [SerializeField] protected string m_localID;

        public string LocalID
        {
            get { return m_localID; }
            set
            {
                if (m_localID != value)
                {
                    m_localID = value;
                    OnUpdateLocal();
                }
            }
        }

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnEnable()
        {
            LocalCfg.RegisterComponent(this);

            if (!string.IsNullOrEmpty(m_localID))
            {
                OnUpdateLocal();
            }
        }

        protected virtual void OnDisable()
        {
            LocalCfg.UnregisterComponent(this);
        }

        protected abstract void OnAwake();

        internal abstract void OnUpdateLocal();

        protected string LocalValue()
        {
            return LocalCfg.GetValue(m_localID);
        }
    }
}