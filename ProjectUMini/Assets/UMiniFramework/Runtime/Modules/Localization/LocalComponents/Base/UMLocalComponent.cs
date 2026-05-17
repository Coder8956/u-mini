using System.Reflection;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Localization.LocalComponents.Base
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

        private static MethodInfo AddLocalComponentMethod = null;
        private static MethodInfo RemoveLocalComponentMethod = null;

        private void Awake()
        {
            if (AddLocalComponentMethod == null)
            {
                AddLocalComponentMethod =
                    UMUtilCommon.GetObjectNoPublicMethod(typeof(UMLocal), "AddLocalComponent");
            }

            if (RemoveLocalComponentMethod == null)
            {
                RemoveLocalComponentMethod =
                    UMUtilCommon.GetObjectNoPublicMethod(typeof(UMLocal), "RemoveLocalComponent");
            }

            AddLocalComponentMethod?.Invoke(UMF.Get<UMLocal>(), new[] {this});
            OnAwake();
            bool legalID = !string.IsNullOrEmpty(m_localID);
            if (legalID)
            {
                OnUpdateLocal();
            }
        }

        private void OnDestroy()
        {
            RemoveLocalComponentMethod?.Invoke(UMF.Get<UMLocal>(), new[] {this});
        }

        protected abstract void OnAwake();

        protected abstract void OnUpdateLocal();

        protected string LocalValue()
        {
            return UMF.Get<UMLocal>().GetLocalValue(m_localID);
        }
    }
}