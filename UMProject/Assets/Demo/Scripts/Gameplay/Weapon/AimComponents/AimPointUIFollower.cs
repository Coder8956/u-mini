using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 瞄准点UI跟随控制器
/// 将 GunAimController 的 m_shootTargetPoint（世界空间）实时转换为UI空间坐标，驱动准星位置。
/// 使用 LateUpdate 确保读取当帧最新值，配合 SmoothDamp 平滑避免跳帧卡顿。
/// 同时管理装弹UI：装弹时显示 TMPReloading 文本，通过 ImgProgress 的 Fill Amount 体现装弹进度。
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class AimPointUIFollower : MonoBehaviour
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

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

    [Header("颜色设置")]
    [Tooltip("射击目标点等于世界瞄准点时的颜色（瞄准一致）")]
    [SerializeField] private Color m_colorOnTargetMatch = Color.yellow;

    [Tooltip("其他情况的颜色（瞄准不一致）")]
    [SerializeField] private Color m_colorOnTargetMismatch = Color.magenta;

    [Header("行为设置")]
    [Tooltip("目标点在相机后方时是否隐藏准星")]
    [SerializeField] private bool m_hideWhenBehindCamera = true;

    [Header("俯仰显示")]
    [Tooltip("用于显示俯仰角的TMP文本（留空则不显示）")]
    [SerializeField] private TMP_Text m_pitchText;

    [Header("装弹UI")]
    [Tooltip("开火控制器（提供装弹状态，留空则自动查找）")]
    [SerializeField] private GunFireController m_gunFireController;

    [Tooltip("装弹文本（装弹时显示，完成时隐藏）")]
    [SerializeField] private TMP_Text m_reloadingText;

    [Tooltip("装弹进度条Image（通过Fill Amount体现进度）")]
    [SerializeField] private Image m_progressImage;

    // ==================== 运行时私有字段 ====================

    private RectTransform m_rectTransform;
    private RectTransform m_parentRectTransform;
    private Canvas m_rootCanvas;
    private Graphic m_graphic;
    private Vector2 m_currentPos;
    private Vector2 m_smoothVelocity;
    private bool m_firstFrame = true;

    // ==================== 生命周期 ====================

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_parentRectTransform = transform.parent as RectTransform;
        m_graphic = GetComponent<Graphic>();

        var canvas = GetComponentInParent<Canvas>();
        m_rootCanvas = canvas != null ? canvas.rootCanvas : null;
    }

    private void Start()
    {
        if (m_gunAimController == null)
            m_gunAimController = FindAnyObjectByType<GunAimController>();

        if (m_gunFireController == null)
            m_gunFireController = FindAnyObjectByType<GunFireController>();

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

        UpdateColor(worldTarget);
        UpdatePitchText(worldTarget);
        UpdateReloadUI();
    }

    // ==================== 瞄准点逻辑 ====================

    /// <summary>
    /// 根据射击目标点是否等于世界瞄准点来更新准星颜色
    /// </summary>
    private void UpdateColor(Vector3 shootTarget)
    {
        if (m_graphic == null) return;

        Vector3 worldAim = m_gunAimController.GetWorldAimPointValue();
        bool isOnTarget = Vector3.Distance(shootTarget, worldAim) < 0.01f;
        Color currentColor = isOnTarget ? m_colorOnTargetMatch : m_colorOnTargetMismatch;
        m_graphic.color = currentColor;

        if (m_pitchText != null)
            m_pitchText.color = currentColor;

        if (m_reloadingText != null)
            m_reloadingText.color = currentColor;
    }

    /// <summary>
    /// 更新TMP文本显示当前炮管俯仰角
    /// </summary>
    private void UpdatePitchText(Vector3 shootTarget)
    {
        if (m_pitchText == null) return;

        int pitch = Mathf.RoundToInt(m_gunAimController.GetCurrentPitch());
        string sign = pitch >= 0 ? "+" : "-";
        m_pitchText.SetText($"Pitch: {sign}{Mathf.Abs(pitch):00}");
    }

    // ==================== 装弹UI逻辑 ====================

    /// <summary>
    /// 更新装弹UI：装弹时显示文本并更新进度条Fill Amount，完成时隐藏
    /// </summary>
    private void UpdateReloadUI()
    {
        if (m_gunFireController == null)
            return;

        bool isReloading = m_gunFireController.IsReloading();

        // TMPReloading 显示/隐藏
        if (m_reloadingText != null)
            m_reloadingText.gameObject.SetActive(isReloading);

        // ImgProgress Fill Amount
        if (m_progressImage != null)
        {
            if (isReloading)
            {
                float reloadTime = m_gunFireController.GetReloadTime();
                float remaining = m_gunFireController.GetRemainingReloadTime();
                float progress = reloadTime > 0f ? 1f - remaining / reloadTime : 1f;
                m_progressImage.fillAmount = progress;
            }
            else
            {
                m_progressImage.fillAmount = 0f;
            }
        }
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

    /// <summary>获取开火控制器</summary>
    public GunFireController GetGunFireController() => m_gunFireController;

    /// <summary>设置开火控制器</summary>
    public void SetGunFireController(GunFireController controller) => m_gunFireController = controller;

    /// <summary>获取装弹文本</summary>
    public TMP_Text GetReloadingText() => m_reloadingText;

    /// <summary>设置装弹文本</summary>
    public void SetReloadingText(TMP_Text text) => m_reloadingText = text;

    /// <summary>获取进度条Image</summary>
    public Image GetProgressImage() => m_progressImage;

    /// <summary>设置进度条Image</summary>
    public void SetProgressImage(Image image) => m_progressImage = image;
}
