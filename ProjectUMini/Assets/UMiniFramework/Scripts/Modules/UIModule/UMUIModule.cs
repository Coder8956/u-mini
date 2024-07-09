using System;
using System.Collections;
using System.Collections.Generic;
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

        private Dictionary<Type, UMUIPanel> m_createdPanel;

        public override IEnumerator Init(UMini.UMiniConfig config)
        {
            m_createdPanel = new Dictionary<Type, UMUIPanel>();
            yield return null;
        }

        public void Open<T>(Action<T> completed = null) where T : UMUIPanel
        {
            UMUIPanelInfo panelInfo = Attribute.GetCustomAttribute(typeof(T), typeof(UMUIPanelInfo)) as UMUIPanelInfo;
            string panelPath = panelInfo.PanelPath;
            // UMUtils.Debug.Log($"panelPath:{panelPath}");

            UMini.Resources.LoadAsync<GameObject>(panelPath, (result) =>
            {
                GameObject panelGO = Instantiate(result.Resource, m_UMUIRootCanvas.transform);
                T panel = panelGO.GetComponent<T>();
                m_createdPanel.Add(panel.GetType(), panel);
                UMUtils.UI.FillParent(panelGO.GetComponent<RectTransform>());
                completed?.Invoke(panel);
            });
        }
    }
}