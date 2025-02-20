using System;
using UnityEngine;
using UnityEngine.UI;

namespace UMiniFramework.Runtime.Modules.UI.Base
{
    public abstract class UMUIPanel : MonoBehaviour
    {
        protected abstract void OnCreatePanel();
        protected abstract void OnDestroyPanel();
        protected abstract void OnOpenPanel();
        protected abstract void OnClosePanel();
    }
}