using UnityEngine;

namespace UMiniFramework.Scripts.Pool.GameObjectPool
{
    /// <summary>
    /// 游戏对象池生成的物体
    /// </summary>
    public class GOPoolObject : MonoBehaviour
    {
        public int RelatedPoolHashTag { get; private set; }

        public void SetRelatedPoolHashTag(int poolHashTag)
        {
            RelatedPoolHashTag = poolHashTag;
            gameObject.name += $"<PoolTag({RelatedPoolHashTag})>";
        }
    }
}