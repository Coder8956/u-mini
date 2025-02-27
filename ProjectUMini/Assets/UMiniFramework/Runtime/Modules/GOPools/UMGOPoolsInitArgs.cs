using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.GOPools
{
    /// <summary>
    /// 事件模块初始化配置
    /// </summary>
    public class UMGOPoolsInitArgs : UMModuleInitArgs
    {
        private int m_poolInitObjectCount = 5;

        /// <summary>
        /// 对象池初始化时生成的对象数量
        /// </summary>
        public int PoolInitObjectCount
        {
            get => m_poolInitObjectCount;
            set => m_poolInitObjectCount = value;
        }
    }
}