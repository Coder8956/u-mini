using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.UI.AttributeUMUI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UMiniFramework.Runtime.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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

        private Color m_panelMaskColor = new(0, 0, 0, 0.8f);

        public Color PanelMaskColor
        {
            get => m_panelMaskColor;
            set => m_panelMaskColor = value;
        }

        private RectTransform m_rectTransform = null;
        private Canvas m_canvas = null;
        public Canvas Canvas => m_canvas;

        private CanvasScaler m_canvasScaler = null;
        public CanvasScaler CanvasScaler => m_canvasScaler;

        private GraphicRaycaster m_graphicRaycaster = null;
        private GameObject m_goEventSystem;
        private List<RectTransform> m_uiLayers = null;
        private Dictionary<int, UMUIPanel> m_panelDic = null;

        private RectTransform m_uiCache = null;

        private int m_createUILayerCount = 7;
        private Camera m_uiCamera;
        public Camera UICamera => m_uiCamera;

        private static FieldInfo Field_Panel_ISOPEN;

        public override UMModuleType ModuleType
        {
            get => UMModuleType.UI;
        }

        public UnityAction<UMUIPanel> OnCreateUI { get; set; }
        public UnityAction<UMUIPanel> OnOpenUI { get; set; }
        public UnityAction<UMUIPanel> OnCloseUI { get; set; }
        public UnityAction<UMUIPanel> OnDestroyUI { get; set; }

        /// <summary>
        /// 顶层 LayerIndex
        /// </summary>
        public int TopLayerIndex
        {
            get { return m_uiLayers.Count - 1; }
        }

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
            m_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        /// <summary>
        /// 创建 CanvasScaler
        /// </summary>
        private void CreateCanvasScaler()
        {
            // 添加 CanvasScaler 组件
            m_canvasScaler = gameObject.AddComponent<CanvasScaler>();
            m_canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            m_canvasScaler.referenceResolution = new Vector2(3840,2160);
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
            int layerCount = m_createUILayerCount < 1 ? 1 : m_createUILayerCount;

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


        protected override IEnumerator Init()
        {
            SetUILayer(gameObject);

            // 添加 RectTransform 组件
            m_rectTransform = gameObject.AddComponent<RectTransform>();

            m_panelDic = new Dictionary<int, UMUIPanel>();
            Field_Panel_ISOPEN = UMUtilCommon.GetObjectNoPublicField(typeof(UMUIPanel), "m_isOpen");
            CreateCanvas();
            CreateCanvasScaler();
            CreateGraphicRaycaster();
            CreateUILayer();
            CreateUICache();
            CreateEventSystem();
            CreateUICamera();
            UMUtilDebug.Log($"{GetType().Name} Inited");

            yield return null;
        }

        private void CreateUICamera()
        {
            GameObject uiCamera = new GameObject("UM_UICamera", typeof(Camera));
            uiCamera.transform.SetParent(transform);
            m_uiCamera = uiCamera.GetComponent<Camera>();
            m_uiCamera.clearFlags = CameraClearFlags.Depth;
            m_uiCamera.depth = 100;
            m_uiCamera.orthographic = true;
            m_uiCamera.nearClipPlane = 0;
            m_uiCamera.farClipPlane = 50;
            m_canvas.renderMode = RenderMode.ScreenSpaceCamera;
            m_canvas.worldCamera = m_uiCamera;
            m_uiCamera.transform.localPosition = new Vector3(0, 0, transform.position.z - 1000);
        }

        /// <summary>
        /// 创建界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Create<T>() where T : UMUIPanel
        {
            // 获取配置特性标签
            UMUIPanelATB uiConfig = (UMUIPanelATB) Attribute.GetCustomAttribute(typeof(T), typeof(UMUIPanelATB));

            T panel = null;
            if (uiConfig.LoadType == UMResLoadType.Resources)
            {
                // 加载界面并设置界面引用值
                panel = ResLoadUI(uiConfig.LoadPath).GetComponent<T>();

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
                panel.transform.localScale = Vector3.one;

                UMUtilUI.FillParent(panel.GetComponent<RectTransform>());
            }
            else
            {
                UMUtilDebug.Warning($"Invalid parameter: {uiConfig.LoadType}");
            }

            OnCreateUI?.Invoke(panel);
            return panel;
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="panel">界面对象</param>
        /// <param name="layerIndex">层级索引</param>
        public void Open(UMUIPanel panel, int layerIndex = 0)
        {
            int layIndex = Mathf.Clamp(layerIndex, 0, m_uiLayers.Count - 1);

            // 设置界面的显示层
            panel.transform.SetParent(m_uiLayers[layIndex]);

            UMUtilUI.FillParent(panel.GetComponent<RectTransform>());

            panel.gameObject.SetActive(true);
            panel.transform.SetAsLastSibling();

            RectTransform panelRT = panel.GetComponent<RectTransform>();
            Vector3 currentPosition = panelRT.localPosition;
            panelRT.localPosition = new Vector3(currentPosition.x, currentPosition.y, 0);

            MethodInfo OnOpenPanelMethod = UMUtilCommon.GetObjectNoPublicMethod(panel.GetType(), "OnOpenPanel");
            Field_Panel_ISOPEN.SetValue(panel, true);
            if (panel.PanelMask != null && panel.IsUseCommonMask)
            {
                panel.PanelMask.color = PanelMaskColor;
            }

            OnOpenPanelMethod.Invoke(panel, null);
            OnOpenUI?.Invoke(panel);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="panel">界面对象</param>
        public void Close(UMUIPanel panel)
        {
            MethodInfo OnClosePanelMethod = UMUtilCommon.GetObjectNoPublicMethod(panel.GetType(), "OnClosePanel");
            OnClosePanelMethod.Invoke(panel, null);
            panel.gameObject.SetActive(false);
            panel.transform.SetParent(m_uiCache);

            Field_Panel_ISOPEN.SetValue(panel, false);
            OnCloseUI?.Invoke(panel);
        }

        /// <summary>
        /// 销毁界面
        /// </summary>
        /// <param name="panel">界面对象</param>
        public void Destroy(UMUIPanel panel)
        {
            MethodInfo OnDestroyPanelMethod = UMUtilCommon.GetObjectNoPublicMethod(panel.GetType(), "OnDestroyPanel");
            OnDestroyPanelMethod.Invoke(panel, null);

            // 获取界面游戏物体的HashCode
            int panelHashCode = panel.gameObject.GetHashCode();

            // 将界面移出字典
            m_panelDic.Remove(panelHashCode);

            OnDestroyUI?.Invoke(panel);
            Destroy(panel.gameObject);
        }

        /// <summary>
        /// 输出所有已经创建的界面
        /// </summary>
        public void DumpCreatedUI()
        {
            int index = 0;
            foreach (var el in m_panelDic)
            {
                UMUtilDebug.Log($"[index:{index}]-({el.Key})-({el.Value.gameObject.name})");
                index++;
            }
        }

        /// <summary>
        /// 判断是否点击了UI
        /// </summary>
        /// <returns></returns>
        public bool IsClickUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}