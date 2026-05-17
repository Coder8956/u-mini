using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Localization.LocalComponents.Base;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Localization
{
    public class UMLocal : UMBaseModule
    {
        private static MethodInfo OnSwitchLocal = null;

        /// <summary>
        /// 本地化字典 [localization,[local_id,value]]
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> m_localDic = null;

        private List<UMLocalComponent> m_localComponents = null;

        public string CurtLocal { get; private set; }

        public override UMModuleType ModuleType
        {
            get => UMModuleType.Localization;
        }

        protected override IEnumerator Init()
        {
            m_localComponents = new List<UMLocalComponent>();
            OnSwitchLocal =
                UMUtilCommon.GetObjectNoPublicMethod(typeof(UMLocalComponent), "OnUpdateLocal");
            UMUtilDebug.Log($"{GetType().Name} Inited");

            yield return null;
        }

        public void SetLocalDic(Dictionary<string, Dictionary<string, string>> dic)
        {
            m_localDic = dic;
        }

        public void SwitchLocal(string local)
        {
            if (CurtLocal == local) return;

            CurtLocal = local;
            for (var i = 0; i < m_localComponents.Count; i++)
            {
                OnSwitchLocal?.Invoke(m_localComponents[i], null);
            }
        }

        private void AddLocalComponent(UMLocalComponent component)
        {
            m_localComponents.Add(component);
        }

        private void RemoveLocalComponent(UMLocalComponent component)
        {
            m_localComponents.Remove(component);
        }

        public string GetLocalValue(string id)
        {
            bool legalID = !string.IsNullOrEmpty(id);
            if (legalID && m_localDic[CurtLocal].ContainsKey(id))
            {
                return m_localDic[CurtLocal][id];
            }
            else
            {
                return "Invalid local id";
            }
        }
    }
}