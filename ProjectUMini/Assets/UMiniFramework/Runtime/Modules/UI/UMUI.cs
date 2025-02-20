using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Game.Scripts.UI.PanelMain;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.UI.Base;
using UMiniFramework.Runtime.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UMiniFramework.Runtime.Modules.UI
{
    /// <summary>
    /// UI模块, 同时也是UI的根Canvas
    /// </summary>
    public class UMUI : UMBaseModule
    {
        private const string EVENT_SYSTEM_NAME = "UM_EventSystem";
        private const string UI_LAYER_PREFIX = "UM_UI_Layer_";
        private const string UI_CACHE = "UM_UI_CACHE";

        private UMUIConfig m_config = null;
        private RectTransform m_rectTransform = null;
        private Canvas m_canvas = null;
        private CanvasScaler m_canvasScaler = null;
        private GraphicRaycaster m_graphicRaycaster = null;
        private GameObject m_goEventSystem;
        private List<RectTransform> m_uiLayers = null;
        private Dictionary<int, UMUIPanel> m_panelDic = null;

        private RectTransform m_uiCache = null;

        private void SetUILayer(GameObject go)
        {
            // 设置为UI层.索引值是5.
            go.layer = 5;
        }

        /// <summary>
        /// 创建事件系统
        /// </summary>
        private void CreateEventSystem()
        {
            if (m_config == null) return;
            if (!m_config.IsCreateEventSystem) return;
            EventSystem es = UMUtilCommon.CreateGameObject<EventSystem>(EVENT_SYSTEM_NAME, gameObject);
            es.AddComponent<StandaloneInputModule>();
            m_goEventSystem = es.gameObject;
        }

        /// <summary>
        /// 创建 Canvas
        /// </summary>
        private void CreateCanvas()
        {
            // 添加 Canvas 组件
            m_canvas = gameObject.AddComponent<Canvas>();
            if (m_config == null)
                m_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            else
                m_canvas.renderMode = m_config.CanvasRenderMode;
        }

        /// <summary>
        /// 创建 CanvasScaler
        /// </summary>
        private void CreateCanvasScaler()
        {
            // 添加 CanvasScaler 组件
            m_canvasScaler = gameObject.AddComponent<CanvasScaler>();
        }

        /// <summary>
        /// 创建 GraphicRaycaster
        /// </summary>
        private void CreateGraphicRaycaster()
        {
            // 添加 GraphicRaycaster 组件
            m_graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
        }

        // 创建UI层级
        private void CreateUILayer()
        {
            m_uiLayers = new List<RectTransform>();
            int layerCount = 1;
            if (m_config != null)
            {
                layerCount = m_config.UILayerCount < 1 ? 1 : m_config.UILayerCount;
            }

            for (int i = 0; i < layerCount; i++)
            {
                RectTransform uiLayerRT = UMUtilCommon.CreateGameObject<RectTransform>(UI_LAYER_PREFIX + i, gameObject);

                UMUtilUI.FillParent(uiLayerRT);

                GameObject uiLayerGo = uiLayerRT.gameObject;
                SetUILayer(uiLayerGo);

                m_uiLayers.Add(uiLayerRT);
            }
        }

        private void CreateUICache()
        {
            m_uiCache = UMUtilCommon.CreateGameObject<RectTransform>(UI_CACHE, gameObject);
            UMUtilUI.FillParent(m_uiCache);
            SetUILayer(m_uiCache.gameObject);
        }

        private GameObject ResLoadUI(string path)
        {
            GameObject uiGo = Resources.Load<GameObject>(path);
            return Instantiate(uiGo);
        }

        protected override IEnumerator Init(UMModuleConfig config)
        {
            m_config = UMUtilCommon.ConvertObjectClass<UMUIConfig>(config);

            SetUILayer(gameObject);

            // 添加 RectTransform 组件
            m_rectTransform = gameObject.AddComponent<RectTransform>();

            m_panelDic = new Dictionary<int, UMUIPanel>();

            CreateCanvas();
            CreateCanvasScaler();
            CreateGraphicRaycaster();
            CreateUILayer();
            CreateUICache();
            CreateEventSystem();

            yield return null;
        }

        /// <summary>
        /// 创建界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Create<T>() where T : UMUIPanel
        {
            // 获取配置特性标签
            UMUIPanelConfig uiConfig =
                (UMUIPanelConfig) Attribute.GetCustomAttribute(typeof(T), typeof(UMUIPanelConfig));

            T panel = null;
            if (uiConfig.PathType == PathEnum.Resources)
            {
                // 加载界面并设置界面引用值
                panel = ResLoadUI(uiConfig.Path).GetComponent<T>();

                // 通过反射调用界面的创建方法
                MethodInfo OnCreatePanel = UMUtilCommon.GetObjectNoPublicMethod(typeof(T), "OnCreatePanel");
                OnCreatePanel.Invoke(panel, null);

                // 失活界面
                panel.gameObject.SetActive(false);

                // 获取界面游戏物体的HashCode
                int panelHashCode = panel.gameObject.GetHashCode();

                // 将界面存入字典
                m_panelDic.Add(panelHashCode, panel);

                // 将界面放入缓存节点
                panel.transform.SetParent(m_uiCache);

                string panelName = panel.gameObject.name.Replace("(Clone)", $"[{panelHashCode}]");
                panel.gameObject.name = panelName;

                UMUtilUI.FillParent(panel.GetComponent<RectTransform>());
            }
            else
            {
                UMUtilDebug.Warning($"Invalid parameter: {uiConfig.PathType}");
            }

            return panel;
        }

        public void Open(UMUIPanel panel, int layerIndex = 0)
        {
            int layIndex = Mathf.Clamp(layerIndex, 0, m_uiLayers.Count - 1);

            // 设置界面的显示层
            panel.transform.SetParent(m_uiLayers[layIndex]);

            UMUtilUI.FillParent(panel.GetComponent<RectTransform>());

            panel.gameObject.SetActive(true);
            panel.transform.SetAsLastSibling();

            MethodInfo OnOpenPanel = UMUtilCommon.GetObjectNoPublicMethod(panel.GetType(), "OnOpenPanel");
            OnOpenPanel.Invoke(panel, null);
        }

        public void Close(UMUIPanel panel)
        {
            panel.gameObject.SetActive(false);
            panel.transform.SetParent(m_uiCache);

            MethodInfo OnClosePanel = UMUtilCommon.GetObjectNoPublicMethod(panel.GetType(), "OnClosePanel");
            OnClosePanel.Invoke(panel, null);
        }

        public void Destroy(UMUIPanel panel)
        {
            MethodInfo OnClosePanel = UMUtilCommon.GetObjectNoPublicMethod(panel.GetType(), "OnClosePanel");
            OnClosePanel.Invoke(panel, null);

            // 获取界面游戏物体的HashCode
            int panelHashCode = panel.gameObject.GetHashCode();

            // 将界面移出字典
            m_panelDic.Remove(panelHashCode);

            Destroy(panel.gameObject);
        }

        public void DumpCreatedUI()
        {
            int index = 0;
            foreach (var el in m_panelDic)
            {
                UMUtilDebug.Log($"[index:{index}]-({el.Key})-({el.Value.gameObject.name})");
                index++;
            }
        }
    }
}