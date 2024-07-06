using System;
using System.Collections;
using UMiniFramework.Scripts.Utils;
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

        public void Open<T>(Action<T> completed = null) where T : UMUIPanel
        {
            UMUIPanelInfo panelInfo = Attribute.GetCustomAttribute(typeof(T), typeof(UMUIPanelInfo)) as UMUIPanelInfo;
            string panelPath = panelInfo.PanelPath;
            UMUtils.Debug.Log($"panelPath:{panelPath}");

            UMini.Resources.LoadAsync<GameObject>(panelPath, (result) =>
            {
                GameObject panelGameObject = Instantiate<GameObject>(result.Resource);
                panelGameObject.transform.SetParent(m_UMUIRootCanvas.transform);
                UMUtils.UI.FillParent(panelGameObject.GetComponent<RectTransform>());
                panelGameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                panelGameObject.GetComponent<RectTransform>().localScale = Vector3.one;
            });
        }
    }
}