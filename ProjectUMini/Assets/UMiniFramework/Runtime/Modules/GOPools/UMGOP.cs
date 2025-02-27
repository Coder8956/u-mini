using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime.Pool.GameObjectPools;
using UMiniFramework.Runtime.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace UMiniFramework.Runtime.Modules.GOPools
{
    /// <summary>
    /// GameObject 对象池
    /// </summary>
    public class UMGOP : MonoBehaviour
    {
        private static FieldInfo Field_UMGOPObject_BornPool;

        public UnityAction<GameObject> OnGet { get; set; }
        public UnityAction<GameObject> OnBack { get; set; }
        private GameObject m_prototype = null;
        private Queue<GameObject> m_goQue;
        private List<GameObject> m_outPoolGos;
        private int m_initObjectCount;

        private void InitPool(GameObject prototype, int initObjectCount)
        {
            if (Field_UMGOPObject_BornPool == null)
            {
                Field_UMGOPObject_BornPool = UMUtilCommon.GetObjectNoPublicField(typeof(UMGOPObject), "m_bornPool");
            }

            m_goQue = new Queue<GameObject>();
            m_outPoolGos = new List<GameObject>();
            m_initObjectCount = Mathf.Clamp(m_initObjectCount, 0, initObjectCount);

            m_prototype = Instantiate(prototype, transform);
            m_prototype.SetActive(false);
            m_prototype.transform.localPosition = Vector3.zero;
            UMGOPObject umgopObject = m_prototype.AddComponent<UMGOPObject>();
            Field_UMGOPObject_BornPool.SetValue(umgopObject, this);

            for (int i = 0; i < m_initObjectCount; i++)
            {
                m_goQue.Enqueue(CreateObject());
            }
        }

        private GameObject CreateObject()
        {
            return Instantiate(m_prototype, transform);
        }

        public GameObject Get()
        {
            GameObject getGo = null;
            if (m_goQue.Count > 0)
            {
                getGo = m_goQue.Dequeue();
            }
            else
            {
                getGo = CreateObject();
            }

            m_outPoolGos.Add(getGo);
            getGo.transform.parent = null;
            OnGet?.Invoke(getGo);
            return getGo;
        }

        public void Back(GameObject backGO)
        {
            if (backGO.GetComponent<UMGOPObject>() == null)
            {
                UMUtilDebug.Error("Not a pool object, cannot go back to the pool.");
                return;
            }

            if (backGO.GetComponent<UMGOPObject>().BornPool != this)
            {
                UMUtilDebug.Error("The object returned to the wrong pool.");
                return;
            }

            OnBack?.Invoke(backGO);
            backGO.SetActive(false);
            backGO.transform.SetParent(transform);
            m_prototype.transform.localPosition = Vector3.zero;
            m_outPoolGos.Remove(backGO);
        }

        private void DestroyPool()
        {
            for (var i = 0; i < m_outPoolGos.Count; i++)
            {
                Destroy(m_outPoolGos[i]);
            }

            Destroy(gameObject);
        }
    }
}