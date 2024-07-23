using System;
using System.Collections;
using System.Collections.Generic;
using UMiniFramework.Scripts.UMEntrance;
using UMiniFramework.Scripts.Utils;

namespace UMiniFramework.Scripts.Modules.ConfigModule
{
    public class UMConfigModule : UMModule
    {
        private Dictionary<Type, UMConfigTable> m_tableDic;

        public T GetTable<T>() where T : UMConfigTable
        {
            Type key = typeof(T);
            if (m_tableDic != null && m_tableDic.ContainsKey(key))
            {
                return m_tableDic[key] as T;
            }
            else
            {
                UMUtils.Debug.Warning($"Table {key.Name} does not exist.");
                return null;
            }
        }

        public override IEnumerator Init(UMiniConfig config)
        {
            if (config.ConfigTableList != null)
            {
                m_tableDic = new Dictionary<Type, UMConfigTable>();
                foreach (var table in config.ConfigTableList)
                {
                    m_tableDic.Add(table.GetType(), table);
                    yield return table.Init();
                }
            }

            yield return null;
        }
    }
}