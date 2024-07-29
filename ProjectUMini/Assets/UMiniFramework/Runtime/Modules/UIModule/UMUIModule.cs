using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMiniFramework.Runtime.UMEntrance;
using UMiniFramework.Runtime.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UMiniFramework.Runtime.Modules.UIModule
{
    public class UMUIModule : UMModule
    {
        [SerializeField] private Canvas m_UMUIRootCanvas = null;
        [SerializeField] private Camera m_UMUICamera = null;
        [SerializeField] private EventSystem m_UMEventSystem = null;
        [SerializeField] private RectTransform m_openedRoot;
        [SerializeField] private RectTransform[] m_UILayerRoots;
        [SerializeField] private RectTransform m_closedRoot;
        private CanvasGroup m_UMUIRootCanvasGroup;
        private Dictionary<string, UMUIPanel> m_createdPanel;

        /// <summary>
        /// 控制UMini全局的UI是否可以交互
        /// </summary>
        public bool Interactable
        {
            get { return m_UMUIRootCanvasGroup.interactable; }
            set { m_UMUIRootCanvasGroup.interactable = value; }
        }

        public override IEnumerator Init(UMiniConfig config)
        {
            m_createdPanel = new Dictionary<string, UMUIPanel>();
            m_UMUIRootCanvasGroup = m_UMUIRootCanvas.GetComponent<CanvasGroup>();
            yield return null;
            m_initFinished = true;
            UMUtilCommon.PrintModuleInitFinishedLog(GetType().Name, m_initFinished);
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
                        if (panel is UMUIDialog)
                        {
                            UMUIDialog dialog = panel as UMUIDialog;
                            dialog.InitMask();
                        }

                        OnPanelOpenHandler(panel, panelInfo, completed);
                    }
                    else
                    {
                        UMUtilDebug.Warning($"Load Panel Failed. Path:{panelInfo.PanelPath}");
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
            UMUtilUI.FillParent(panelRectTrans);
            panel.OnOpen();
            completed?.Invoke(panel as T);
        }

        public void Close<T>()
        {
            UMUIPanelInfo panelInfo = GetPanelInfo<T>();
            if (m_createdPanel.Keys.Contains(panelInfo.PanelPath))
            {
                UMUIPanel closePanel = m_createdPanel[panelInfo.PanelPath];
                closePanel.gameObject.SetActive(false);
                closePanel.OnClose();
                closePanel.GetComponent<RectTransform>().SetParent(m_closedRoot);
            }
        }

        public void Close(UMUIPanel panel)
        {
            UMUIPanelInfo panelInfo = GetPanelInfo(panel);
            if (m_createdPanel.Keys.Contains(panelInfo.PanelPath))
            {
                UMUIPanel closePanel = m_createdPanel[panelInfo.PanelPath];
                closePanel.gameObject.SetActive(false);
                closePanel.OnClose();
                closePanel.GetComponent<RectTransform>().SetParent(m_closedRoot);
            }
        }

        private UMUIPanelInfo GetPanelInfo<T>()
        {
            UMUIPanelInfo panelInfo = Attribute.GetCustomAttribute(typeof(T), typeof(UMUIPanelInfo)) as UMUIPanelInfo;
            return panelInfo;
        }

        private UMUIPanelInfo GetPanelInfo(UMUIPanel panel)
        {
            UMUIPanelInfo panelInfo =
                Attribute.GetCustomAttribute(panel.GetType(), typeof(UMUIPanelInfo)) as UMUIPanelInfo;
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