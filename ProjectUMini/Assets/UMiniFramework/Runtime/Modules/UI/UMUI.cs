using System.Collections;
using UMiniFramework.Runtime.Modules.Base;
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

        private UMUIConfig m_config = null;
        private RectTransform m_rectTransform = null;
        private Canvas m_canvas = null;
        private CanvasScaler m_canvasScaler = null;
        private GraphicRaycaster m_graphicRaycaster = null;
        private GameObject m_goEventSystem;

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
            if (m_config == null) return;
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

        public override IEnumerator Init(UMModuleConfig config)
        {
            m_config = UMUtilCommon.ConvertObjectClass<UMUIConfig>(config);

            // 设置为UI层.索引值是5.
            gameObject.layer = 5;

            // 添加 RectTransform 组件
            m_rectTransform = gameObject.AddComponent<RectTransform>();

            CreateCanvas();
            CreateCanvasScaler();
            CreateGraphicRaycaster();
            CreateEventSystem();
            yield return null;
        }
    }
}