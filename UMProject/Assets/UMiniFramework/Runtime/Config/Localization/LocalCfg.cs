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
        /// 语言选项列表
        /// </summary>
        private List<LangOption> m_localOptions;

        /// <summary>
        /// code → type 映射
        /// </summary>
        private Dictionary<string, string> m_codeToType;

        private List<UMLocalComponent> m_localComponents;

        public string CurtType { get; private set; }

        public string CurtCode { get; private set; }

        // ── 静态访问（通过 UMConfig.Local 委托） ──

        private static LocalCfg Instance => UMConfig.IsCreated ? UMConfig.Local : null;

        private static bool IsCreated => Instance != null;

        private void Awake()
        {
            m_localComponents = new List<UMLocalComponent>();
        }

        internal void SetLocalData(
            List<LangOption> options,
            Dictionary<string, Dictionary<string, string>> content)
        {
            if (options == null || content == null)
                throw new ArgumentNullException();

            m_localDic = content;
            m_localOptions = options;
            m_codeToType = new Dictionary<string, string>(options.Count);
            foreach (var opt in options)
            {
                if (!string.IsNullOrEmpty(opt.code))
                    m_codeToType[opt.code] = opt.type;
            }
        }

        public List<LangOption> GetOptions()
        {
            return m_localOptions;
        }

        public void SwitchByType(string type)
        {
            if (string.IsNullOrEmpty(type) || CurtType == type)
                return;

            if (m_localDic == null || !m_localDic.ContainsKey(type))
            {
                Debug.LogWarning($"[LocalCfg] SwitchByType 失败：未找到语言 '{type}'。");
                return;
            }

            CurtType = type;
            CurtCode = FindCodeByType(type);

            NotifyComponents();
        }

        public void SwitchByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return;

            if (m_codeToType == null || !m_codeToType.TryGetValue(code, out string type))
            {
                Debug.LogWarning($"[LocalCfg] SwitchByCode 失败：未找到语言代码 '{code}'。");
                return;
            }

            if (CurtType == type)
                return;

            CurtType = type;
            CurtCode = code;

            NotifyComponents();
        }

        private string FindCodeByType(string type)
        {
            if (m_localOptions == null)
                return null;

            for (int i = 0; i < m_localOptions.Count; i++)
            {
                if (m_localOptions[i].type == type)
                    return m_localOptions[i].code;
            }
            return null;
        }

        private void NotifyComponents()
        {
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
            if (string.IsNullOrEmpty(id) || m_localDic == null || string.IsNullOrEmpty(CurtType))
                return string.Empty;

            if (m_localDic.TryGetValue(CurtType, out var localSubDic) &&
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