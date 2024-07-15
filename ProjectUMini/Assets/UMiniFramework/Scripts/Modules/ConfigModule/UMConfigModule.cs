using System;
using System.Collections;
using System.Collections.Generic;
using UMiniFramework.Scripts.Utils;

namespace UMiniFramework.Scripts.Modules.ConfigModule
{
    public class UMConfigModule : UMModule
    {
        private Dictionary<Type, UMConfigTable> m_tableDic;

        public override IEnumerator Init(UMini.UMiniConfig config)
        {
            if (config.ConfigTableList != null)
            {
                m_tableDic = new Dictionary<Type, UMConfigTable>();
                foreach (var table in config.ConfigTableList)
                {
                    StartCoroutine(table.Init());
                    m_tableDic.Add(table.GetType(), table);
                }
            }

            yield return null;
        }
    }
}