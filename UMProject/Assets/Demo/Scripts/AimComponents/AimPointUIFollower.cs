using UnityEngine;

/// <summary>
/// 瞄准点UI跟随控制器
/// 将 GunAimController 的 m_shootTargetPoint（世界空间）实时转换为UI空间坐标，驱动准星位置。
/// 使用 LateUpdate 确保读取当帧最新值，配合 SmoothDamp 平滑避免跳帧卡顿。
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class AimPointUIFollower : MonoBehaviour
{
    [Header("引用")]
    [Tooltip("大炮瞄准控制器（留空则自动查找场景中的 GunAimController）")]
    [SerializeField] private GunAimController m_gunAimController;

    [Tooltip("用于世界→屏幕坐标转换的相机（留空则使用 Canvas 渲染相机，再退回 Camera.main）")]
    [SerializeField] private Camera m_worldCamera;

    [Header("平滑设置")]
    [Tooltip("是否启用位置平滑插值")]
    [SerializeField] private bool m_enableSmoothing = true;

    [Tooltip("平滑时间（秒）—— 值越小跟随越紧")]
    [SerializeField] private float m_smoothTime = 0.06f;

    [Header("行为设置")]
    [Tooltip("目标点在相机后方时是否隐藏准星")]
    [SerializeField] private bool m_hideWhenBehindCamera = true;

    // ==================== 运行时私有字段 ====================

    private RectTransform m_rectTransform;
    private RectTransform m_parentRectTransform;
    private Canvas m_rootCanvas;
    private Vector2 m_currentPos;
    private Vector2 m_smoothVelocity;
    private bool m_firstFrame = true;

    // ==================== 生命周期 ====================

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_parentRectTransform = transform.parent as RectTransform;

        var canvas = GetComponentInParent<Canvas>();
        m_rootCanvas = canvas != null ? canvas.rootCanvas : null;
    }

    private void Start()
    {
        if (m_gunAimController == null)
            m_gunAimController = FindAnyObjectByType<GunAimController>();

        // 首帧初始化位置，避免从 (0,0) 滑动到目标位置
        if (m_gunAimController != null && TryGetUILocalPoint(m_gunAimController.GetShootTarget(), out Vector2 initPos))
        {
            m_currentPos = initPos;
            m_rectTransform.anchoredPosition = initPos;
            m_firstFrame = false;
        }
    }

    /// <summary>
    /// 在 LateUpdate 中执行，确保 GunAimController.Update() 已计算完当帧的射击目标点
    /// </summary>
    private void LateUpdate()
    {
        if (m_gunAimController == null || m_parentRectTransform == null) return;

        Vector3 worldTarget = m_gunAimController.GetShootTarget();

        if (!TryGetUILocalPoint(worldTarget, out Vector2 targetUIPos))
        {
            if (m_hideWhenBehindCamera)
                m_rectTransform.gameObject.SetActive(false);
            return;
        }

        if (!m_rectTransform.gameObject.activeSelf)
            m_rectTransform.gameObject.SetActive(true);

        if (m_enableSmoothing && !m_firstFrame)
        {
            m_currentPos = Vector2.SmoothDamp(m_currentPos, targetUIPos, ref m_smoothVelocity, m_smoothTime);
        }
        else
        {
            m_currentPos = targetUIPos;
            m_firstFrame = false;
        }

        m_rectTransform.anchoredPosition = m_currentPos;
    }

    // ==================== 坐标转换 ====================

    /// <summary>
    /// 将世界坐标转换为父节点的 UI 本地坐标
    /// </summary>
    private bool TryGetUILocalPoint(Vector3 worldPoint, out Vector2 localPoint)
    {
        localPoint = Vector2.zero;

        Camera cam = ResolveCamera();
        if (cam == null) return false;

        Vector3 screenPoint = cam.WorldToScreenPoint(worldPoint);

        // z < 0 表示目标在相机后方
        if (screenPoint.z < 0) return false;

        // Overlay Canvas 传 null（按像素坐标处理），Camera 模式传渲染相机
        Camera uiCamera = m_rootCanvas != null ? m_rootCanvas.worldCamera : null;
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_parentRectTransform, screenPoint, uiCamera, out localPoint);
    }

    /// <summary>
    /// 解析用于世界→屏幕坐标转换的相机：
    /// 优先使用 Inspector 指定的相机 → Canvas 渲染相机 → Camera.main
    /// </summary>
    private Camera ResolveCamera()
    {
        if (m_worldCamera != null) return m_worldCamera;
        if (m_rootCanvas != null && m_rootCanvas.worldCamera != null) return m_rootCanvas.worldCamera;
        return Camera.main;
    }

    // ==================== 公开接口 ====================

    /// <summary>设置跟随的大炮瞄准控制器</summary>
    public void SetGunAimController(GunAimController controller) => m_gunAimController = controller;
}
