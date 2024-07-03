using UnityEngine;

namespace UMiniFramework.Scripts.Kit.Pool
{
    public class PoolTool
    {
        public static GameObjectPool CreateGameObjectPool(string poolName, GameObject parent = null)
        {
            GameObjectPool goPool = UMTool.CreateGameObject<GameObjectPool>(parent);
            goPool.gameObject.name = poolName;
            return goPool.GetComponent<GameObjectPool>();
        }
    }
}