using System.Collections;
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

        private UMUIConfig m_config = null;
        private RectTransform m_rectTransform = null;
        private Canvas m_canvas = null;
        private CanvasScaler m_canvasScaler = null;
        private GraphicRaycaster m_graphicRaycaster = null;
        private GameObject m_goEventSystem;

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
            int layerCount = 1;
            if (m_config != null)
            {
                layerCount = m_config.UILayerCount < 1 ? 1 : m_config.UILayerCount;
            }

            for (int i = 0; i < layerCount; i++)
            {
                RectTransform uiLayerRT = UMUtilCommon.CreateGameObject<RectTransform>(UI_LAYER_PREFIX + i, gameObject);

                // 修改锚点
                uiLayerRT.anchorMin = Vector2.zero;
                uiLayerRT.anchorMax = Vector2.one;

                // 修改边界偏移量
                uiLayerRT.offsetMin = Vector2.zero;
                uiLayerRT.offsetMax = Vector2.zero;

                GameObject uiLayerGo = uiLayerRT.gameObject;
                SetUILayer(uiLayerGo);
            }
        }


        public override IEnumerator Init(UMModuleConfig config)
        {
            m_config = UMUtilCommon.ConvertObjectClass<UMUIConfig>(config);

            SetUILayer(gameObject);

            // 添加 RectTransform 组件
            m_rectTransform = gameObject.AddComponent<RectTransform>();

            CreateCanvas();
            CreateCanvasScaler();
            CreateGraphicRaycaster();
            CreateUILayer();
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
            return null;
        }
    }
}