using System.Collections;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Base
{
    /// <summary>
    /// UM模块基类
    /// </summary>
    public abstract class UMBaseModule : MonoBehaviour
    {
        private UMModuleConfig m_config = null;
        public abstract IEnumerator Init(UMModuleConfig config);
    }
}