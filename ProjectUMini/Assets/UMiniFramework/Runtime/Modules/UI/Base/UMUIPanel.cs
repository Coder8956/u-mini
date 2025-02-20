using System;
using UnityEngine;
using UnityEngine.UI;

namespace UMiniFramework.Runtime.Modules.UI.Base
{
    public abstract class UMUIPanel : MonoBehaviour
    {
        /// <summary>
        /// 在创建的时候执行一次
        /// </summary>
        protected abstract void OnCreatePanel();
        
        /// <summary>
        /// 在销毁的时候执行一次
        /// </summary>
        protected abstract void OnDestroyPanel();
        
        /// <summary>
        /// 每次打开的时候执行
        /// </summary>
        protected abstract void OnOpenPanel();
        
        /// <summary>
        /// 每次关闭的时候执行
        /// </summary>
        protected abstract void OnClosePanel();
    }
}