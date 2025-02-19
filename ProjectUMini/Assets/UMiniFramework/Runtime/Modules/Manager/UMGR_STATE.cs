using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Manager
{
    public enum UMGR_STATE
    {
        /// <summary>
        /// 无效值
        /// </summary>
        INVALID,
        
        /// <summary>
        /// 初始化完成
        /// </summary>
        INITED,
        
        /// <summary>
        /// 启动中
        /// </summary>
        LAUNCHING,
        
        /// <summary>
        /// 启动完成
        /// </summary>
        LAUNCHED
    }
}