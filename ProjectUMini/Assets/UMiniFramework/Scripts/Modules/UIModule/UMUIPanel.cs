using UnityEngine;

namespace UMiniFramework.Scripts.Modules.UIModule
{
    public abstract class UMUIPanel : MonoBehaviour
    {
        public abstract void OnLoaded();
        public abstract void OnOpen();
        public abstract void OnClose();
    }
}