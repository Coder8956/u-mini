using System;
using System.Collections.Generic;
using UMiniFramework.Scripts.Utils;
using UnityEngine;

namespace UMiniFramework.Scripts.Pool.GameObjectPool
{
    public class UMGameObjectPool : MonoBehaviour
    {
        /// <summary>
        /// 池内对象数量
        /// </summary>
        public int ObjectCount
        {
            get { return (m_gameObjectQue != null ? m_gameObjectQue.Count : 0); }
        }

        private Queue<GameObject> m_gameObjectQue;
        private UMPoolConfig m_poolConfig;
        private int m_createdNum = 0;
        private const string OBJECT_TEMPLET_TAG = "[TEMPLET]";
        private int m_hashTag = 0;

        private void Init(UMPoolConfig poolConfig)
        {
            m_poolConfig = poolConfig;
            m_gameObjectQue = new Queue<GameObject>();
            m_poolConfig.ObjectTemplet.transform.SetParent(transform);
            m_poolConfig.ObjectTemplet.name += OBJECT_TEMPLET_TAG;
            m_poolConfig.ObjectTemplet.SetActive(false);

            m_hashTag = GetHashCode();
            // UMUtils.Debug.Log($"Pool HashTag:{m_hashTag}");
            gameObject.name += $"<HashTag({m_hashTag})>";

            for (int i = 0; i < poolConfig.InitNum; i++)
            {
                m_gameObjectQue.Enqueue(Create());
            }
        }

        private GameObject Create()
        {
            GameObject newObject = Instantiate(m_poolConfig.ObjectTemplet, gameObject.transform);
            m_createdNum++;
            UMGOPoolObject poolObject = newObject.AddComponent<UMGOPoolObject>();
            poolObject.SetRelatedPool(m_hashTag, this);
            string oldName = newObject.name;
            newObject.name = oldName.Replace($"{OBJECT_TEMPLET_TAG}(Clone)", $"_{m_createdNum}");
            return newObject;
        }

        public GameObject Get()
        {
            GameObject obj = null;
            if (m_gameObjectQue.Count > 0)
            {
                obj = m_gameObjectQue.Dequeue();
            }
            else
            {
                obj = Create();
            }

            obj.SetActive(true);
            m_poolConfig.OnGet?.Invoke(obj);
            return obj;
        }

        public void Back(GameObject obj)
        {
            int relatedPoolTag = obj.GetComponent<UMGOPoolObject>().RelatedPoolHashTag;
            if (m_hashTag != relatedPoolTag)
            {
                throw new Exception("The object returned to the wrong object pool");
            }

            m_poolConfig.OnBack?.Invoke(obj);
            m_gameObjectQue.Enqueue(obj);
            obj.SetActive(false);
            obj.transform.SetParent(transform, false);
        }

        public static UMGameObjectPool CreatePool(UMPoolConfig poolConfig)
        {
            UMGameObjectPool newGameObjectPool =
                UMUtilCommon.CreateGameObject<UMGameObjectPool>(poolConfig.PoolName, poolConfig.PoolParent);
            newGameObjectPool.Init(poolConfig);
            return newGameObjectPool;
        }

        public class UMPoolConfig
        {
            public readonly string PoolName;
            public readonly GameObject PoolParent;
            public readonly GameObject ObjectTemplet;
            public readonly int InitNum;
            public readonly Action<GameObject> OnGet;
            public readonly Action<GameObject> OnBack;

            public UMPoolConfig(
                string poolName,
                GameObject poolParent,
                GameObject objectTemplet,
                int initNum,
                Action<GameObject> onGet,
                Action<GameObject> onBack
            )
            {
                PoolName = poolName;
                PoolParent = poolParent;
                ObjectTemplet = objectTemplet;
                InitNum = initNum;
                OnGet = onGet;
                OnBack = onBack;
            }
        }
    }
}