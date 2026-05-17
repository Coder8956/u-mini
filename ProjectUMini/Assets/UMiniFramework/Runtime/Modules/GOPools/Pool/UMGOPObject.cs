using UnityEngine;

namespace UMiniFramework.Runtime.Modules
{
    /// <summary>
    /// 游戏对象池生成的物体
    /// </summary>
    public class UMGOPObject : MonoBehaviour
    {
        private UMGOP m_bornPool;

        public UMGOP BornPool => m_bornPool;

        // public void BackBornPool()
        // {
        //     m_bornPool.Back(gameObject);
        // }
    }
}