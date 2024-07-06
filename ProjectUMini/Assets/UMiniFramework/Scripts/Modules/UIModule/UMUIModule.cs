using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UMiniFramework.Scripts.Modules.UIModule
{
    public class UMUIModule : UMModule
    {
        [SerializeField] private Canvas m_UMUIRootCanvas = null;
        [SerializeField] private Camera m_UMUICamera = null;
        [SerializeField] private EventSystem m_UMEventSystem = null;

        public override IEnumerator Init(UMini.UMiniConfig config)
        {
            yield return null;
        }
    }
}