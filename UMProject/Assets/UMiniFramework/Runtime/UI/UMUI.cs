using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UMiniFramework.Runtime
{
    public class UMUI : UMMonoSingleton<UMUI>
    {
        private const string CanvasName = "UMUI-Canvas";
        private const string EventSystemName = "UMUI-EventSystem";
        private const string UICacheName = "UMUI-UICache";
        private static List<RectTransform> UILayers;
        private static readonly Dictionary<Type, List<UMUIPanelBase>> CachePanels = new();
        private Transform m_UICache;

        /// <summary>
        /// UI最顶层
        /// </summary>
        public static int UIMaxLayer
        {
            get
            {
                int index = UILayers?.Count - 1 ?? 0;
                return index;
            }
        }

        public static Canvas Canvas { get; private set; }
        public static CanvasScaler CanvasScaler { get; private set; }
        public static EventSystem EventSystem { get; private set; }

        public static Color PanelMaskColor { get; set; }

        internal static Transform UICache => Instance.m_UICache;

        /// <summary>
        /// 创建新的 UI 面板实例并放入缓存，但不打开。
        /// 每次调用都会创建新实例，支持同类型多实例。
        /// 调用者通过返回的实例自行 Open / Close / Release。
        /// </summary>
        public static T Create<T>() where T : UMUIPanelBase
        {
            Type type = typeof(T);

            UMUIPanelCfg cfg = type.GetCustomAttribute<UMUIPanelCfg>();
            if (cfg == null || string.IsNullOrEmpty(cfg.PrefabPath))
            {
                Debug.LogError($"[UMUI] {type.Name} 缺少 UMUIPanelCfg 特性或 PrefabPath 为空");
                return null;
            }

            GameObject prefab = Resources.Load<GameObject>(cfg.PrefabPath);
            if (prefab == null)
            {
                Debug.LogError($"[UMUI] 无法加载 Prefab: {cfg.PrefabPath}");
                return null;
            }

            GameObject go = Instantiate(prefab, UICache, false);
            go.name = prefab.name;
            T panel = go.GetComponent<T>();
            if (panel == null)
            {
                Debug.LogError($"[UMUI] Prefab 上未找到组件: {type.Name}");
                Destroy(go);
                return null;
            }

            panel.Initialize();
            go.SetActive(false);
            // UMUIUtils.StretchFull(panel.GetComponent<RectTransform>());

            // 加入缓存（支持同类型多实例）
            if (!CachePanels.TryGetValue(type, out List<UMUIPanelBase> list))
            {
                list = new List<UMUIPanelBase>();
                CachePanels[type] = list;
            }

            list.Add(panel);

            return panel;
        }

        // ── 供 UMUIPanelBase 内部使用的辅助方法 ──────────────────

        internal static RectTransform GetLayer(int index)
        {
            if (index < 0 || index >= UILayers.Count)
                return UILayers[0];
            return UILayers[index];
        }

        internal static void RemoveFromCache(UMUIPanelBase panel)
        {
            if (panel == null) return;

            Type type = panel.GetType();
            if (CachePanels.TryGetValue(type, out List<UMUIPanelBase> list))
            {
                list.Remove(panel);
                if (list.Count == 0)
                    CachePanels.Remove(type);
            }
        }

        protected override void OnInit()
        {
            Canvas = CreateCanvas();
            EventSystem = CreateEventSystem();
            UILayers = CreateUILayers();
            m_UICache = CreateUICache();
            PanelMaskColor = new Color(0, 0, 0, 0.8f);
        }

        private static Canvas CreateCanvas()
        {
            // Canvas
            GameObject go = new GameObject(CanvasName);
            Canvas canvas = go.AddComponent<Canvas>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // UI缩放
            CanvasScaler = go.AddComponent<CanvasScaler>();
            CanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            CanvasScaler.referenceResolution = new Vector2(1920, 1080);
            CanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            CanvasScaler.matchWidthOrHeight = 0f;

            // GraphicRaycaster
            go.AddComponent<GraphicRaycaster>();
            go.transform.SetParent(Instance.transform);
            return canvas;
        }

        private static EventSystem CreateEventSystem()
        {
            GameObject go = new GameObject(EventSystemName);
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
            go.transform.SetParent(Instance.transform);
            return go.GetComponent<EventSystem>();
        }

        private static List<RectTransform> CreateUILayers()
        {
            List<RectTransform> layers = new List<RectTransform>();
            for (int i = 0; i < 9; i++)
            {
                GameObject newLayer = new GameObject($"layer-{i}", typeof(RectTransform));
                newLayer.transform.SetParent(Canvas.transform, false);
                UMUIUtils.StretchFull(newLayer.GetComponent<RectTransform>());
                layers.Add(newLayer.GetComponent<RectTransform>());
            }

            return layers;
        }

        private static Transform CreateUICache()
        {
            GameObject UICacheGo = new GameObject(UICacheName, typeof(RectTransform));
            UICacheGo.transform.SetParent(Canvas.transform, false);
            return UICacheGo.transform;
        }
    }
}