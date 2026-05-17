using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.GOPools.Pool
{
    /// <summary>
    /// GameObject 对象池
    /// </summary>
    public class UMGOP : MonoBehaviour
    {
        private static FieldInfo Field_UMGOPObject_BornPool;
        private GameObject m_prototype = null;
        private const string PrototypeTag = "prototype";
        private string m_poolTag = string.Empty;
        private Queue<GameObject> m_goQue;
        private List<GameObject> m_outPoolGos;
        private int m_initObjectCount;

        private string m_objectName = string.Empty;

        private void InitPool(string poolTag, GameObject prototype, int initObjectCount)
        {
            if (Field_UMGOPObject_BornPool == null)
            {
                Field_UMGOPObject_BornPool = UMUtilCommon.GetObjectNoPublicField(typeof(UMGOPObject), "m_bornPool");
            }

            m_poolTag = poolTag;
            m_goQue = new Queue<GameObject>();
            m_outPoolGos = new List<GameObject>();

            m_initObjectCount = initObjectCount;
            m_initObjectCount = Mathf.Clamp(m_initObjectCount, 0, int.MaxValue);

            m_prototype = Instantiate(prototype, transform);
            m_objectName = m_prototype.name.Replace("(Clone)", "");
            m_prototype.name = string.Concat(m_objectName, "-", PrototypeTag);
            m_prototype.SetActive(false);
            m_prototype.transform.localPosition = Vector3.zero;
            m_prototype.AddComponent<UMGOPObject>();

            for (int i = 0; i < m_initObjectCount; i++)
            {
                m_goQue.Enqueue(CreateObject());
            }
        }

        private GameObject CreateObject()
        {
            GameObject cgo = Instantiate(m_prototype, transform);

            UMGOPObject umgopObject = cgo.GetComponent<UMGOPObject>();
            Field_UMGOPObject_BornPool.SetValue(umgopObject, this);
            cgo.name = string.Concat(m_objectName, $"-HashCode[{cgo.GetHashCode()}]");
            return cgo;
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
            getGo.transform.SetParent(null);
            getGo.GetComponent<IUMGOPObjectFunc>()?.OnGet();
            getGo.SetActive(true);
            return getGo;
        }

        public void Back(GameObject backGO)
        {
            UMGOPObject umgopObject = backGO.GetComponent<UMGOPObject>();

            if (umgopObject == null)
            {
                UMUtilDebug.Error("Not a pool object, cannot go back to the pool.");
                return;
            }

            UMGOP bornUMGOP = (UMGOP) Field_UMGOPObject_BornPool.GetValue(umgopObject);
            if (bornUMGOP != this)
            {
                UMUtilDebug.Error("The object returned to the wrong pool.");
                return;
            }

            backGO.GetComponent<IUMGOPObjectFunc>()?.OnBack();
            backGO.SetActive(false);
            backGO.transform.SetParent(transform);
            backGO.transform.localPosition = Vector3.zero;
            m_outPoolGos.Remove(backGO);
            m_goQue.Enqueue(backGO);
        }

        private void DestroyPool()
        {
            for (var i = 0; i < m_outPoolGos.Count; i++)
            {
                Destroy(m_outPoolGos[i]);
            }

            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}