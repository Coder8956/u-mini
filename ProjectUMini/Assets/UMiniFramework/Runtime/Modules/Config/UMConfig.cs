using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Config.Base;
using UMiniFramework.Runtime.Modules.Config.Interface;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Config
{
    public class UMConfig : UMBaseModule
    {
        private ILoadConfigHandler m_loadConfigHandler;
        private UMConfigInitArgs m_initArgs = null;
        private Dictionary<Type, UMConfigTable> m_tableDic;
        private MethodInfo m_tableInit = null;
        private MethodInfo m_handlerLoad = null;

        public override UMModuleType ModuleType
        {
            get => UMModuleType.Config;
        }

        public T GetTable<T>() where T : UMConfigTable
        {
            Type key = typeof(T);
            if (m_tableDic != null && m_tableDic.ContainsKey(key))
            {
                return m_tableDic[key] as T;
            }
            else
            {
                UMUtilDebug.Warning($"Table {key.Name} does not exist.");
                return null;
            }
        }

        protected override IEnumerator Init(UMModuleInitArgs initArgs)
        {
            m_initArgs = UMUtilCommon.ConvertObjectClass<UMConfigInitArgs>(initArgs);
            if (m_initArgs.LoadConfigHandler == null)
            {
                UMUtilDebug.Error("m_initArgs.LoadConfigHandler is null");
            }

            m_tableDic = new Dictionary<Type, UMConfigTable>();
            m_loadConfigHandler = m_initArgs.LoadConfigHandler;

            m_handlerLoad = UMUtilCommon.GetObjectNoPublicMethod(typeof(ILoadConfigHandler), "LoadConfig");

            if (m_initArgs.ConfigTables != null)
            {
                for (var i = 0; i < m_initArgs.ConfigTables.Count; i++)
                {
                    UMConfigTable table = m_initArgs.ConfigTables[i];
                    m_tableInit = UMUtilCommon.GetObjectNoPublicMethod(table.GetType(), "Init");
                    string result = (string) m_handlerLoad.Invoke(m_loadConfigHandler, new object[] {table.LoadPath});
                    m_tableInit.Invoke(table, new object[] {result});
                    m_tableDic.Add(table.GetType(), table);
                }
            }

            yield return null;
        }
    }
}