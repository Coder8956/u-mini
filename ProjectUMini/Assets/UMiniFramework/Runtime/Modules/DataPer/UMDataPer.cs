using System;
using System.Collections;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.UMDataPer.Base;
using UMiniFramework.Runtime.Utils;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.UMDataPer
{
    public class UMDataPer : UMBaseModule
    {
        public string SaveRootDir { get; set; }
        private FieldInfo m_dataCreateTimeField;
        private FieldInfo m_dataSaveTimeField;

        /// <summary>
        /// 加密处理
        /// </summary>
        public Func<string, string> EncryptionHandler { get; set; }

        /// <summary>
        /// 解密处理
        /// </summary>
        public Func<string, string> DecryptionHandler { get; set; }

        public override UMModuleType ModuleType
        {
            get => UMModuleType.DataPer;
        }

        protected override IEnumerator Init()
        {
            SaveRootDir = Application.streamingAssetsPath;

            if (!Directory.Exists(SaveRootDir))
            {
                Directory.CreateDirectory(SaveRootDir);
            }

            m_dataCreateTimeField = UMUtilCommon.GetObjectNoPublicField(typeof(UMBaseDataPerObject), "CreateTime");
            m_dataSaveTimeField = UMUtilCommon.GetObjectNoPublicField(typeof(UMBaseDataPerObject), "LastSaveTime");

            UMUtilDebug.Log($"{GetType().Name} Inited");
            yield return null;
        }

        private string GetDataFileFullPath(string fileName)
        {
            return Path.Combine(SaveRootDir, fileName).Replace('\\', '/');
        }

        private string GetTime()
        {
            return DateTime.Now.ToString("yyyy-M-d HH:mm:ss");
        }

        public T Create<T>(T defaultVal = null) where T : UMBaseDataPerObject
        {
            T data = defaultVal;
            if (data == null)
            {
                // 创建带参数的实例
                // Type type = typeof(MyClassWithParameters);
                // object instance = Activator.CreateInstance(type, "参数1", 123);

                // 创建无参构造函数的实例
                data = (T) Activator.CreateInstance(typeof(T));
            }

            m_dataCreateTimeField.SetValue(data, GetTime());
            return data;
        }

        public void Delete(string name)
        {
            string fileFullPath = GetDataFileFullPath(name);
            if (File.Exists(fileFullPath))
            {
                File.Delete(fileFullPath);
#if UNITY_EDITOR
                File.Delete($"{fileFullPath}.meta");
                AssetDatabase.Refresh();
#endif
            }
        }

        public void DeleteAll()
        {
            Directory.Delete(SaveRootDir, true);
            Directory.CreateDirectory(SaveRootDir);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public void Save<T>(string name, T data) where T : UMBaseDataPerObject
        {
            string fileFullPath = GetDataFileFullPath(name);
            string fileDir = Path.GetDirectoryName(fileFullPath);
            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }

            m_dataSaveTimeField.SetValue(data, GetTime());
            string jsonStr = JsonConvert.SerializeObject(data, Formatting.Indented);

            if (IsValidEncrypDecryp())
            {
                jsonStr = EncryptionHandler.Invoke(jsonStr);
            }

            File.WriteAllText(fileFullPath, jsonStr);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public T Read<T>(string name) where T : UMBaseDataPerObject
        {
            T data = null;
            string fileFullPath = GetDataFileFullPath(name);
            if (File.Exists(fileFullPath))
            {
                string jsonContent = File.ReadAllText(fileFullPath);
                if (IsValidEncrypDecryp())
                {
                    jsonContent = DecryptionHandler.Invoke(jsonContent);
                    // Debug.Log(jsonContent);
                }

                data = JsonConvert.DeserializeObject<T>(jsonContent);
            }
            else
            {
                // UMUtilDebug.Warning($"Read data <{name}> failed!");
            }

            return data;
        }

        private bool IsValidEncrypDecryp()
        {
            return (EncryptionHandler != null && DecryptionHandler != null);
        }

        public void PrintSaveRootDir()
        {
            Debug.Log($"UMDataPer SaveRootDir: {GetDataFileFullPath("")}");
        }
    }
}