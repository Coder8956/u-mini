using UnityEngine;

namespace UMiniFramework.Runtime.Pool.GameObjectPool
{
    /// <summary>
    /// 游戏对象池生成的物体
    /// </summary>
    public class UMGOPoolObject : MonoBehaviour
    {
        public int RelatedPoolHashTag { get; private set; }
        public UMGameObjectPool RelatedPool { get; private set; }

        public void SetRelatedPool(int poolHashTag, UMGameObjectPool relatedPool)
        {
            RelatedPoolHashTag = poolHashTag;
            RelatedPool = relatedPool;
            gameObject.name += $"<PoolTag({RelatedPoolHashTag})>";
        }

        public void BackPool()
        {
            RelatedPool.Back(gameObject);
        }
    }
}