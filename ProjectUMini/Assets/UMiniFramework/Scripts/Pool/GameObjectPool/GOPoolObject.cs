using UnityEngine;

namespace UMiniFramework.Scripts.Pool.GameObjectPool
{
    /// <summary>
    /// 游戏对象池生成的物体
    /// </summary>
    public class GOPoolObject : MonoBehaviour
    {
        public int RelatedPoolHashTag { get; private set; }
        public GameObjectPool RelatedPool { get; private set; }

        public void SetRelatedPool(int poolHashTag, GameObjectPool relatedPool)
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