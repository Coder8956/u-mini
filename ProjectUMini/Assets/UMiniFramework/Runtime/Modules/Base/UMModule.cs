using System.Collections;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Base
{
    /// <summary>
    /// UM模块基类
    /// </summary>
    public abstract class UMBaseModule : MonoBehaviour
    {
        public abstract IEnumerator Init();
    }
}