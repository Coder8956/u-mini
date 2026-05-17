using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Config.Base;
using UMiniFramework.Runtime.Modules.Config.UMLoadConfigHandlers;
using UMiniFramework.Runtime.Modules.Config.UMLoadConfigHandlers.Interface;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Config
{
    public class UMConfig : UMBaseModule
    {
        private IUMLoadConfigHandler m_loadConfigHandler;
        private Dictionary<Type, UMConfigTable> m_tableDic = new();
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
                UMUtilDebug.Warning($"The <{key.Name}> cannot be obtained. Please register the <{key.Name}> first.");
                return null;
            }
        }

        public void AddTable<T>(T table) where T : UMConfigTable
        {
            Type tableKey = table.GetType();
            if (m_tableDic.ContainsKey(tableKey)) return;
            m_tableInit = UMUtilCommon.GetObjectNoPublicMethod(table.GetType(), "Init");
            string result = (string) m_handlerLoad.Invoke(m_loadConfigHandler, new object[] {table.LoadPath});
            m_tableInit.Invoke(table, new object[] {result});
            m_tableDic.Add(tableKey, table);
        }

        protected override IEnumerator Init()
        {
            m_handlerLoad = UMUtilCommon.GetObjectNoPublicMethod(typeof(IUMLoadConfigHandler), "LoadConfig");
            m_loadConfigHandler = new UMResLoadConfigHandler();
            UMUtilDebug.Log($"{GetType().Name} Inited");
            yield return null;
        }
    }
}