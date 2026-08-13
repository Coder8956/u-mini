using System;
using System.Collections.Generic;
using UnityEngine;

namespace UMiniFramework.Runtime
{
    public class UMConfig : UMMonoSingleton<UMConfig>
    {
        private LocalCfg m_local;

        /// <summary>
        /// 多语言功能对象（添加语言配置表时自动创建）
        /// </summary>
        public static LocalCfg Local => IsCreated ? Instance.m_local : null;

        private readonly Dictionary<Type, UMBaseConfigTable> m_tableDic = new();

        private static Dictionary<Type, UMBaseConfigTable> TableDic = null;

        protected override void OnInit()
        {
            TableDic = Instance.m_tableDic;
        }

        /// <summary>
        /// 获取配置表
        /// </summary>
        public static T GetTable<T>() where T : UMBaseConfigTable
        {
            Type key = typeof(T);

            if (TableDic.TryGetValue(key, out UMBaseConfigTable table))
            {
                return table as T;
            }

            Debug.LogWarning($"Config table not found : {key.Name}");
            return null;
        }


        /// <summary>
        /// 添加配置表
        /// </summary>
        public static bool AddTable<T>(T table) where T : UMBaseConfigTable
        {
            if (table == null)
            {
                Debug.LogError("Add Config Table Failed : table is null");
                return false;
            }

            Type tableType = typeof(T);

            if (TableDic.ContainsKey(tableType))
            {
                Debug.LogWarning($"Config table already exists : {tableType.Name}");
                return false;
            }

            TextAsset asset = Resources.Load<TextAsset>(table.LoadPath);

            if (asset == null)
            {
                Debug.LogError(
                    $"Config file not found : {table.LoadPath}"
                );

                return false;
            }

            table.Init(asset.text);

            TableDic.Add(tableType, table);

            // 多语言配置表自动创建并初始化 LocalCfg
            if (table is IUMLangTable langTable)
            {
                if (Instance.m_local == null)
                {
                    var go = new GameObject("LocalCfg");
                    go.transform.SetParent(Instance.transform);
                    Instance.m_local = go.AddComponent<LocalCfg>();
                }

                var options = langTable.GetOptions();
                var localDic = new Dictionary<string, Dictionary<string, string>>(options.Count);
                for (int i = 0; i < options.Count; i++)
                {
                    localDic[options[i].type] = langTable.GetContent(options[i].type);
                }
                Instance.m_local.SetLocalData(options, localDic);
            }

            return true;
        }

        /// <summary>
        /// 移除配置表
        /// </summary>
        public static bool RemoveTable<T>() where T : UMBaseConfigTable
        {
            return TableDic.Remove(typeof(T));
        }

        /// <summary>
        /// 清空所有配置
        /// </summary>
        public static void Clear()
        {
            TableDic.Clear();
        }
    }
}