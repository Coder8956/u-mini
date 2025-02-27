using UMiniFramework.Runtime.Modules.GOPools;
using UnityEngine;

namespace UMiniFramework.Runtime.Pool.GameObjectPools
{
    /// <summary>
    /// 游戏对象池生成的物体
    /// </summary>
    public class UMGOPObject : MonoBehaviour
    {
        private UMGOP m_bornPool;
        public UMGOP BornPool => m_bornPool;
    }
}