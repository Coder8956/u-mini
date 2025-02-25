using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UMiniFramework.Runtime.Modules.DataPer.Interface;
using UMiniFramework.Runtime.Utils;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.DataPer.UMDataPerHandlers
{
    public class UMDataJsonFileHandler : IUMDataPerHandler
    {
        private const string DATA_FILE = "um_data_persistence.json";
        private string m_dateDirectory = string.Empty;
        private string m_jsonFilePath = string.Empty;
        private Dictionary<string, string> m_dataDic;

        void IUMDataPerHandler.Init()
        {
            m_dateDirectory = Path.Combine(Application.streamingAssetsPath, "UMData");
            m_jsonFilePath = Path.Combine(m_dateDirectory, DATA_FILE);
            m_jsonFilePath = UMUtilIO.FormatPathSeparator(m_jsonFilePath);

            // UMUtilDebug.Log($"UMDataJsonFileHandler JsonFilePath: {m_jsonFilePath}");

            UMUtilIO.CreateDir(m_dateDirectory);

            if (UMUtilIO.IsExistsFile(m_jsonFilePath))
            {
                string jsonContent = UMUtilIO.FileReadAllText(m_jsonFilePath);
                m_dataDic = DeserializeToJson<Dictionary<string, string>>(jsonContent);
            }
            else
            {
                m_dataDic = new Dictionary<string, string>();
                SaveJsonFile();
            }

            UMUtilDebug.Log($"UMDataJsonFileHandler Save Path: {m_jsonFilePath}");
        }

        private void SaveJsonFile()
        {
            string jsonContent = ConvertJsonString(m_dataDic);
            UMUtilIO.FileWriteAllText(m_jsonFilePath, jsonContent);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public static string ConvertJsonString(object obj)
        {
            string jsonStr = JsonConvert.SerializeObject(obj, Formatting.Indented);
            jsonStr = Regex.Unescape(jsonStr);
            return jsonStr;
        }

        public static T DeserializeToJson<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        void IUMDataPerHandler.Save(string key, string val)
        {
            if (m_dataDic.ContainsKey(key))
            {
                m_dataDic[key] = val;
            }
            else
            {
                m_dataDic.Add(key, val);
            }

            SaveJsonFile();
        }

        string IUMDataPerHandler.Read(string key, string defaultVal)
        {
            if (m_dataDic.ContainsKey(key))
            {
                return m_dataDic[key];
            }
            else
            {
                return defaultVal;
            }
        }

        void IUMDataPerHandler.Delete(string key)
        {
            if (m_dataDic.ContainsKey(key))
            {
                m_dataDic.Remove(key);
                SaveJsonFile();
            }
        }

        void IUMDataPerHandler.DeleteAll()
        {
            m_dataDic.Clear();
            SaveJsonFile();
        }
    }
}