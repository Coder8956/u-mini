using System.Collections;
using UnityEngine;

namespace UMiniFramework.Scripts.Modules
{
    public abstract class UMModule : MonoBehaviour
    {
        /// <summary>
        /// 初始化优先级
        /// </summary>
        public int InitPriority { get; set; }

        public abstract IEnumerator Init(UMini.UMiniConfig config);
    }
}