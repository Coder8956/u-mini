using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [Header("鼠标锁定设置")]
    [Tooltip("是否隐藏鼠标指针")]
    [SerializeField] private bool hideCursor = true;

    [Tooltip("锁定模式：Locked 用于第一人称/FPS（固定在屏幕中心）；Confined 用于限制在窗口内")]
    [SerializeField] private CursorLockMode lockMode = CursorLockMode.Locked;

    [Header("按键解锁设置")]
    [Tooltip("按下 Esc 键时是否允许临时解锁鼠标")]
    [SerializeField] private bool allowUnlockWithEscape = true;

    private bool isLocked;
    // 延迟一帧应用锁定，规避 Win11 窗口焦点冲突
    private bool pendingLock;

    private void Start()
    {
        pendingLock = true;
    }

    private void Update()
    {
        // 延迟一帧应用，等待窗口初始化完成
        if (pendingLock)
        {
            pendingLock = false;
            ApplyCursorState(true);
        }

        // 鼠标左键点击：重新锁定鼠标
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && !isLocked)
        {
            ApplyCursorState(true);
        }

        // Esc 键临时释放鼠标（方便调试或打开菜单）
        if (allowUnlockWithEscape && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
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
            pendingLock = true;
    }

    /// <summary>
    /// 统一设置鼠标状态
    /// </summary>
    public void ApplyCursorState(bool shouldLock)
    {
        isLocked = shouldLock;

        if (shouldLock)
        {
            Cursor.visible = !hideCursor;
            Cursor.lockState = lockMode;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnDestroy()
    {
        // 脚本销毁或切换场景时还原鼠标
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}