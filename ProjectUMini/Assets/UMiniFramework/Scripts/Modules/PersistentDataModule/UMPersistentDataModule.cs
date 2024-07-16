using System.Collections;
using UMiniFramework.Scripts.Utils;
using UnityEngine;

namespace UMiniFramework.Scripts.Modules.PersistentDataModule
{
    public class UMPersistentDataModule : UMModule
    {
        [SerializeField] private Color m_logColor = Color.yellow;
        private string m_colorStr;
        private bool m_isPersiDataToConsole = true;

        /// <summary>
        /// 数据转换
        /// </summary>
        private IDataConverter m_dataConverter;

        /// <summary>
        /// 数据持久化
        /// </summary>
        private IDataPersistenceHandler m_dataPers;

        public override IEnumerator Init(UMini.UMiniConfig config)
        {
            m_dataConverter = config.DataConverter;
            m_dataPers = config.DataPers;
            m_isPersiDataToConsole = config.IsPersiDataToConsole;
            m_colorStr = ColorUtility.ToHtmlStringRGB(m_logColor);
            yield return null;
        }

        private bool IsPrintLog()
        {
            return (m_isPersiDataToConsole);
        }

        /// <summary>
        /// 获取持久化数据
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="defaultVal">默认值</param>
        /// <typeparam name="T">基础数据类型都可以,引用类型需要支持可序列化</typeparam>
        /// <returns></returns>
        public T Get<T>(string key, T defaultVal)
        {
            string val = m_dataPers.Read(key, string.Empty);

            if (val == string.Empty)
            {
                if (IsPrintLog())
                {
                    string log = $"[Get] key:{key}; return default val: {defaultVal}";
                    UMUtils.Debug.Log($"<color=#{m_colorStr}>{log}</color>");
                }

                return defaultVal;
            }
            else
            {
                if (IsPrintLog())
                {
                    string log = $"[Get] key:{key}; val:{val}";
                    UMUtils.Debug.Log($"<color=#{m_colorStr}>{log}</color>");
                }

                return m_dataConverter.Decoder<T>(val);
            }
        }

        /// <summary>
        /// 保存持久化数据
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="val">数据值</param>
        /// <typeparam name="T">基础数据类型都可以,引用类型需要支持可序列化</typeparam>
        public void Save<T>(string key, T val)
        {
            string valStr = m_dataConverter.Encoder<T>(val);
            if (IsPrintLog())
            {
                string log = $"[Save] key:{key}; val:{valStr}";
                UMUtils.Debug.Log($"<color=#{m_colorStr}>{log}</color>");
            }

            m_dataPers.Write(key, valStr);
        }

        /// <summary>
        /// 删除持久化数据
        /// </summary>
        /// <param name="key">key值</param>
        public void Delete(string key)
        {
            if (IsPrintLog())
            {
                string log = $"[Delete] key:{key}";
                UMUtils.Debug.Log($"<color=#{m_colorStr}>{log}</color>");
            }

            m_dataPers.Delete(key);
        }

        /// <summary>
        /// 清空所有持久化数据
        /// </summary>
        public void Clear()
        {
            if (IsPrintLog())
                UMUtils.Debug.Log($"<color=#{m_colorStr}>[Clear]</color>");

            m_dataPers.Clear();
        }
    }
}