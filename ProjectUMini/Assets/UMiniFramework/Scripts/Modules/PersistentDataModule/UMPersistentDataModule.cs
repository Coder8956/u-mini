using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UMiniFramework.Scripts.UMEntrance;
using UMiniFramework.Scripts.Utils;
using UnityEditor;

namespace UMiniFramework.Scripts.Modules.PersistentDataModule
{
    public class UMPersistentDataModule : UMModule
    {
        private bool m_isPersiDataToConsole = true;
        private Dictionary<string, UMPersistentData> m_persistentDataDic;
        private const string m_persiDataFileExtend = ".json";
        private string m_persiDataRootPath;

        public override IEnumerator Init(UMiniConfig config)
        {
            m_persiDataRootPath = UMPersistentDataRootDir.GetRootDir();
            m_persistentDataDic = new Dictionary<string, UMPersistentData>();
            m_isPersiDataToConsole = config.IsPersiDataToConsole;

            UMUtilIO.CreateDir(m_persiDataRootPath);

            if (config.PersistentDataList != null)
            {
                for (var i = 0; i < config.PersistentDataList.Count; i++)
                {
                    UMPersistentData data = config.PersistentDataList[i];
                    string key = GetDataKey(data);
                    m_persistentDataDic.Add(key, InitPersistentData(key, data));
                }
            }

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            yield return null;
            m_initFinished = true;
            UMUtilCommon.PrintModuleInitFinishedLog(GetType().Name, m_initFinished);
        }

        private UMPersistentData InitPersistentData(string key, UMPersistentData defaultData)
        {
            UMPersistentData PersiData = null;
            string filePath = GetDataFilePath(key);
            if (UMUtilIO.IsExistsFile(filePath))
            {
                string jsonStr = UMUtilIO.FileReadAllText(filePath);
                PersiData = JsonConvert.DeserializeObject(jsonStr, defaultData.GetType()) as UMPersistentData;
            }
            else
            {
                string jsonStr = defaultData.ToJson();
                UMUtilIO.FileWriteAllText(filePath, jsonStr);
                PersiData = defaultData;
            }

            return PersiData;
        }

        private string GetDataFilePath(string fileName)
        {
            return string.Concat(m_persiDataRootPath, $"/{fileName}", m_persiDataFileExtend);
        }

        private string GetDataKey(UMPersistentData data)
        {
            return data.GetType().Name;
        }

        private string GetDataKey<T>() where T : UMPersistentData, new()
        {
            return typeof(T).Name;
        }

        private bool IsPrintLog()
        {
            return (m_isPersiDataToConsole);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Query<T>() where T : UMPersistentData, new()
        {
            string key = GetDataKey<T>();
            if (m_persistentDataDic.ContainsKey(key))
            {
                return m_persistentDataDic[key] as T;
            }
            else
            {
                UMUtilDebug.Warning($"UMPersistentDataDic The key [{key}] does not exist.");
                return null;
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Save<T>() where T : UMPersistentData, new()
        {
            string key = GetDataKey<T>();
            if (m_persistentDataDic.ContainsKey(key))
            {
                string filePath = GetDataFilePath(key);
                string jsonStr = m_persistentDataDic[key].ToJson();
                if (IsPrintLog())
                {
                    string log = $"[Save] key:{key};\n{jsonStr}";
                    UMUtilDebug.Log(log);
                }

                UMUtilIO.FileWriteAllText(filePath, jsonStr);
            }
            else
            {
                UMUtilDebug.Warning($"UMPersistentDataDic The key [{key}] does not exist.");
            }
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ResetDefault<T>() where T : UMPersistentData, new()
        {
            string key = GetDataKey<T>();
            if (m_persistentDataDic.ContainsKey(key))
            {
                m_persistentDataDic[key] = new T();
            }
            else
            {
                UMUtilDebug.Warning($"UMPersistentDataDic The key [{key}] does not exist.");
            }
        }

        /// <summary>
        /// 保存所有数据
        /// </summary>
        public void SaveAllData()
        {
            foreach (var kv in m_persistentDataDic)
            {
                string filePath = GetDataFilePath(kv.Key);
                string jsonStr = kv.Value.ToJson();
                if (IsPrintLog())
                {
                    string log = $"[SaveAllData] key:{kv.Key};\n{jsonStr}";
                    UMUtilDebug.Log(log);
                }

                UMUtilIO.FileWriteAllText(filePath, jsonStr);
            }
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
}