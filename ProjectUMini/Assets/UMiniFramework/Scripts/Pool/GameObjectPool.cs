using UMiniFramework.Scripts.Kit;
using UnityEngine;

namespace UMiniFramework.Scripts.Pool
{
    public class GameObjectPool : MonoBehaviour, IPool<GameObject>
    {
        public static GameObjectPool CreatePool(string poolName, GameObject parent = null)
        {
            return UMTool.CreateGameObject<GameObjectPool>(poolName, parent);
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