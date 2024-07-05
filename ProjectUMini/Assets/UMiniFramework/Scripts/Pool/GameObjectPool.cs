using System.Collections.Generic;
using UMiniFramework.Scripts.Kit;
using UnityEngine;

namespace UMiniFramework.Scripts.Pool
{
    public class GameObjectPool : MonoBehaviour
    {
        private Queue<GameObject> m_gameObject;

        public static GameObjectPool CreatePool(string poolName, GameObject parent = null)
        {
            GameObjectPool newGameObjectPool = UMTool.CreateGameObject<GameObjectPool>(poolName, parent);
            newGameObjectPool.Init();
            return newGameObjectPool;
        }

        private void Init()
        {
            m_gameObject = new Queue<GameObject>();
        }

        public GameObject Get()
        {
            throw new System.NotImplementedException();
        }

        public void Back(GameObject obj)
        {
            throw new System.NotImplementedException();
        }
    }
}