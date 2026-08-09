using System;
using System.Collections.Generic;
using UnityEngine;

namespace UMiniFramework.Runtime
{
    /// <summary>
    /// 多语言功能对象，作为 UMConfig 的子 GameObject 运行
    /// </summary>
    public class LocalCfg : MonoBehaviour
    {
        /// <summary>
        /// 本地化字典 [localization,[local_id,value]]
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> m_localDic;

        /// <summary>
        /// 本地化选项
        /// </summary>
        private List<string> m_localOptions;

        private List<UMLocalComponent> m_localComponents;

        public string CurtLocal { get; private set; }

        // ── 静态访问（通过 UMConfig.Local 委托） ──

        private static LocalCfg Instance => UMConfig.IsCreated ? UMConfig.Local : null;

        private static bool IsCreated => Instance != null;

        private void Awake()
        {
            m_localComponents = new List<UMLocalComponent>();
        }

        internal void SetLocalDic(Dictionary<string, Dictionary<string, string>> dic)
        {
            if (dic == null)
                throw new ArgumentNullException(nameof(dic));

            m_localDic = dic;
            m_localOptions = new List<string>(dic.Keys);
        }

        public List<string> GetLocalOptions()
        {
            return m_localOptions;
        }

        public void SwitchLocal(string local)
        {
            if (string.IsNullOrEmpty(local) || CurtLocal == local)
                return;

            if (m_localDic == null || !m_localDic.ContainsKey(local))
            {
                Debug.LogWarning($"[LocalCfg] SwitchLocal 失败：未找到语言 '{local}'。");
                return;
            }

            CurtLocal = local;

            // 快照迭代，防止 OnUpdateLocal 回调中组件注销导致跳过元素
            var snapshot = new List<UMLocalComponent>(m_localComponents);
            for (var i = 0; i < snapshot.Count; i++)
            {
                snapshot[i].OnUpdateLocal();
            }
        }

        internal void RegisterLocalComponent(UMLocalComponent component)
        {
            if (!m_localComponents.Contains(component))
                m_localComponents.Add(component);
        }

        internal void UnregisterLocalComponent(UMLocalComponent component)
        {
            m_localComponents.Remove(component);
        }

        private string GetLocalValue(string id)
        {
            if (string.IsNullOrEmpty(id) || m_localDic == null || string.IsNullOrEmpty(CurtLocal))
                return string.Empty;

            if (m_localDic.TryGetValue(CurtLocal, out var localSubDic) &&
                localSubDic.TryGetValue(id, out var value))
            {
                return value;
            }

            return string.Empty;
        }

        // ── 静态便捷方法（供 UMLocalComponent 使用，无需直接访问 Instance） ──

        internal static void RegisterComponent(UMLocalComponent component)
        {
            if (IsCreated)
                Instance.RegisterLocalComponent(component);
        }

        internal static void UnregisterComponent(UMLocalComponent component)
        {
            if (IsCreated)
                Instance.UnregisterLocalComponent(component);
        }

        internal static string GetValue(string id)
        {
            return IsCreated ? Instance.GetLocalValue(id) : string.Empty;
        }
    }
}