using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules
{
    /// <summary>
    /// GameObject 对象池管理模块
    /// </summary>
    public class UMGOPools : UMBaseModule
    {
        // private UMGOPoolsInitArgs m_initArgs = null;
        private const int PoolInitObjectCount = 5;
        private Dictionary<string, UMGOP> m_poolDic;
        private MethodInfo m_poolInitMethod;
        private MethodInfo m_poolDestroyMethod;

        public override UMModuleType ModuleType
        {
            get => UMModuleType.Pools;
        }

        protected override IEnumerator Init()
        {
            m_poolDic = new Dictionary<string, UMGOP>();
            m_poolInitMethod = UMUtilCommon.GetObjectNoPublicMethod(typeof(UMGOP), "InitPool");
            m_poolDestroyMethod = UMUtilCommon.GetObjectNoPublicMethod(typeof(UMGOP), "DestroyPool");
            UMUtilDebug.Log($"{GetType().Name} Inited");
            yield return null;
        }

        public UMGOP CreatePool(string poolTag, GameObject prototype, int initObjectCount = PoolInitObjectCount)
        {
            if (m_poolDic.ContainsKey(poolTag))
            {
                UMUtilDebug.Warning($"Failed to create a pool. The poolTag [{poolTag}] is repeated");
                return null;
            }

            GameObject newPoolGO = new GameObject(poolTag, typeof(UMGOP));
            newPoolGO.transform.SetParent(transform);
            newPoolGO.transform.position = Vector3.zero;
            UMGOP poolComponent = newPoolGO.GetComponent<UMGOP>();
            m_poolDic.Add(poolTag, poolComponent);
            m_poolInitMethod.Invoke(poolComponent, new object[] {poolTag, prototype, initObjectCount});
            return poolComponent;
        }

        public UMGOP GetPool(string poolTag)
        {
            UMGOP pool = null;
            if (m_poolDic.ContainsKey(poolTag))
            {
                pool = m_poolDic[poolTag];
            }

            return pool;
        }

        public void DestroyPool(string poolTag)
        {
            if (m_poolDic.ContainsKey(poolTag))
            {
                UMGOP pool = m_poolDic[poolTag];
                m_poolDestroyMethod.Invoke(pool, null);
                m_poolDic.Remove(poolTag);
            }
        }
    }
}