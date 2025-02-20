using System;
using UnityEngine;
using UnityEngine.UI;

namespace UMiniFramework.Runtime.Modules.UI.Base
{
    public abstract class UMUIPanel : MonoBehaviour
    {
        public abstract void OnCreatePanel();
        public abstract void OnDestroyPanel();
        public abstract void OnOpenPanel();
        public abstract void OnClosePanel();
    }
}