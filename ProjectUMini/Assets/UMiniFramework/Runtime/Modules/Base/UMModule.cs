using System.Collections;
using UMiniFramework.Runtime.Common;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Base
{
    /// <summary>
    /// UM模块基类
    /// </summary>
    public abstract class UMBaseModule : MonoBehaviour
    {
        public abstract UMModuleType ModuleType { get; }
        protected abstract IEnumerator Init(UMModuleInitArgs initArgs);
    }
}