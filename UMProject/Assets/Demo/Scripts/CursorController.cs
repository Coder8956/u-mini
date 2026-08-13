using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

    [Header("鼠标锁定设置")]
    [Tooltip("是否隐藏鼠标指针")]
    [SerializeField] private bool m_hideCursor = true;

    [Tooltip("锁定模式：Locked 用于第一人称/FPS（固定在屏幕中心）；Confined 用于限制在窗口内")]
    [SerializeField] private CursorLockMode m_lockMode = CursorLockMode.Locked;

    [Header("按键解锁设置")]
    [Tooltip("按下 Esc 键时是否允许临时解锁鼠标")]
    [SerializeField] private bool m_allowUnlockWithEscape = true;

    // ==================== 私有字段（运行时状态） ====================

    private bool m_isLocked;
    // 延迟一帧应用锁定，规避 Win11 窗口焦点冲突
    private bool m_pendingLock;

    // ==================== 生命周期 ====================

    private void Start()
    {
        m_pendingLock = true;
    }

    private void Update()
    {
        // 延迟一帧应用，等待窗口初始化完成
        if (m_pendingLock)
        {
            m_pendingLock = false;
            ApplyCursorState(true);
        }

        // 鼠标左键点击：重新锁定鼠标
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && !m_isLocked)
        {
            ApplyCursorState(true);
        }

        // Esc 键临时释放鼠标（方便调试或打开菜单）
        if (m_allowUnlockWithEscape && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ApplyCursorState(false);
        }
    }

    /// <summary>
    /// 窗口重获焦点时标记延迟锁定，下一帧 Update 中统一处理
    /// </summary>
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
            m_pendingLock = true;
    }

    private void OnDestroy()
    {
        // 脚本销毁或切换场景时还原鼠标
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // ==================== 公开接口 ====================

    /// <summary>
    /// 统一设置鼠标状态
    /// </summary>
    public void ApplyCursorState(bool shouldLock)
    {
        m_isLocked = shouldLock;

        if (shouldLock)
        {
            Cursor.visible = !m_hideCursor;
            Cursor.lockState = m_lockMode;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
