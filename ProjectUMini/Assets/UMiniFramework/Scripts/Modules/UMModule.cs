using System.Collections;
using UnityEngine;

namespace UMiniFramework.Scripts.Modules
{
    public abstract class UMModule : MonoBehaviour
    {
        public abstract IEnumerator Init(UMini.UMiniConfig config);
    }
}