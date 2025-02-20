using System.Collections;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.BaseModule
{
    public abstract class UMBaseModule : MonoBehaviour
    {
        public abstract IEnumerator Init();
    }
}