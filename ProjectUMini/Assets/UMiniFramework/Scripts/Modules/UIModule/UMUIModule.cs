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
        [SerializeField] private RectTransform m_closedRoot;
        private Dictionary<string, UMUIPanel> m_createdPanel;

        public override IEnumerator Init(UMini.UMiniConfig config)
        {
            m_createdPanel = new Dictionary<string, UMUIPanel>();
            yield return null;
        }

        public void Open<T>(Action<T> completed = null) where T : UMUIPanel
        {
            string panelPath = GetPanelPath<T>();
            // UMUtils.Debug.Log($"panelPath:{panelPath}");

            T panel = null;

            if (m_createdPanel.Keys.Contains(panelPath))
            {
                // 从缓存中获取 界面
                panel = m_createdPanel[panelPath] as T;
                OnPanelOpenHandler(panel, completed);
            }
            else
            {
                // 加载界面
                UMini.Resources.LoadAsync<GameObject>(panelPath, (result) =>
                {
                    if (result.State)
                    {
                        GameObject panelGO = Instantiate(result.Resource, m_UMUIRootCanvas.transform);
                        panel = panelGO.GetComponent<T>();
                        m_createdPanel.Add(panelPath, panel);
                        panel.OnLoaded();
                        OnPanelOpenHandler(panel, completed);
                    }
                    else
                    {
                        UMUtils.Debug.Warning($"Load Panel Failed. Path:{panelPath}");
                    }
                });
            }
        }

        private void OnPanelOpenHandler<T>(UMUIPanel panel, Action<T> completed = null) where T : UMUIPanel
        {
            if (!panel.gameObject.activeSelf)
            {
                panel.gameObject.SetActive(true);
            }

            RectTransform panelRectTrans = panel.GetComponent<RectTransform>();
            panelRectTrans.SetParent(m_openedRoot);
            UMUtils.UI.FillParent(panelRectTrans);
            panel.OnOpen();
            completed?.Invoke(panel as T);
        }

        public void Close<T>()
        {
            string panelPath = GetPanelPath<T>();
            if (m_createdPanel.Keys.Contains(panelPath))
            {
                UMUIPanel panel = m_createdPanel[panelPath];
                panel.gameObject.SetActive(false);
                panel.OnClose();
                panel.GetComponent<RectTransform>().SetParent(m_closedRoot);
            }
        }

        private string GetPanelPath<T>()
        {
            UMUIPanelInfo panelInfo = Attribute.GetCustomAttribute(typeof(T), typeof(UMUIPanelInfo)) as UMUIPanelInfo;
            return panelInfo.PanelPath;
        }
    }
}