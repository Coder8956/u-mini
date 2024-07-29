using System.Collections;
using UMiniFramework.Runtime.UMEntrance;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules
{
    public abstract class UMModule : MonoBehaviour
    {
        /// <summary>
        /// 初始化优先级
        /// </summary>
        public int InitPriority { get; set; }

        protected bool m_initFinished = false;

        /// <summary>
        /// 完成模块初始化
        /// </summary>
        public bool InitFinished
        {
            get { return m_initFinished; }
        }

        public abstract IEnumerator Init(UMiniConfig config);
    }
}