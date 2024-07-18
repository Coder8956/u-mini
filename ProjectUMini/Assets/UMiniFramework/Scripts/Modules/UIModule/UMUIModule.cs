using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        [SerializeField] private RectTransform m_openedRoot;
        [SerializeField] private RectTransform[] m_UILayerRoots;
        [SerializeField] private RectTransform m_closedRoot;
        private Dictionary<string, UMUIPanel> m_createdPanel;

        public override IEnumerator Init(UMini.UMiniConfig config)
        {
            m_createdPanel = new Dictionary<string, UMUIPanel>();
            yield return null;
        }

        public void Open<T>(Action<T> completed = null) where T : UMUIPanel
        {
            UMUIPanelInfo panelInfo = GetPanelInfo<T>();
            // UMUtils.Debug.Log($"panelPath:{panelPath}");

            T panel = null;

            if (m_createdPanel.Keys.Contains(panelInfo.PanelPath))
            {
                // 从缓存中获取 界面
                panel = m_createdPanel[panelInfo.PanelPath] as T;
                OnPanelOpenHandler(panel, panelInfo, completed);
            }
            else
            {
                // 加载界面
                UMini.Asset.LoadAsync<GameObject>(panelInfo.PanelPath, (result) =>
                {
                    if (result.State)
                    {
                        GameObject panelGO = Instantiate(result.Resource, m_UMUIRootCanvas.transform);
                        panel = panelGO.GetComponent<T>();
                        m_createdPanel.Add(panelInfo.PanelPath, panel);
                        panel.OnLoaded();
                        OnPanelOpenHandler(panel, panelInfo, completed);
                    }
                    else
                    {
                        UMUtils.Debug.Warning($"Load Panel Failed. Path:{panelInfo.PanelPath}");
                    }
                });
            }
        }

        private void OnPanelOpenHandler<T>(UMUIPanel panel, UMUIPanelInfo info, Action<T> completed = null)
            where T : UMUIPanel
        {
            if (!panel.gameObject.activeSelf)
            {
                panel.gameObject.SetActive(true);
            }

            RectTransform panelRectTrans = panel.GetComponent<RectTransform>();
            int uiLayer = (int) info.Layer;
            panelRectTrans.SetParent(m_UILayerRoots[uiLayer]);
            UMUtils.UI.FillParent(panelRectTrans);
            panel.OnOpen();
            completed?.Invoke(panel as T);
        }

        public void Close<T>()
        {
            UMUIPanelInfo panelInfo = GetPanelInfo<T>();
            if (m_createdPanel.Keys.Contains(panelInfo.PanelPath))
            {
                UMUIPanel panel = m_createdPanel[panelInfo.PanelPath];
                panel.gameObject.SetActive(false);
                panel.OnClose();
                panel.GetComponent<RectTransform>().SetParent(m_closedRoot);
            }
        }

        private UMUIPanelInfo GetPanelInfo<T>()
        {
            UMUIPanelInfo panelInfo = Attribute.GetCustomAttribute(typeof(T), typeof(UMUIPanelInfo)) as UMUIPanelInfo;
            return panelInfo;
        }

        public bool IsPointerOverUIObject()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }
    }
}