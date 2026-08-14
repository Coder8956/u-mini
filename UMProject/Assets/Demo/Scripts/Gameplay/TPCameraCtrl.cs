using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 第三人称相机控制器（坦克世界风格）
/// 相机始终围绕目标旋转，鼠标移动控制视角，滚轮控制距离，附带碰撞检测防止穿墙
/// </summary>
public class TPCameraCtrl : MonoBehaviour
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

    [Header("输入控制")] [Tooltip("是否接受用户输入（false时鼠标和滚轮操作无效）")] [SerializeField]
    private bool m_enableInput = true;

    [Header("编辑器同步")] [Tooltip("开启时将初始化数值直接同步到Transform")] [SerializeField]
    private bool m_syncInitToTransform;

    [Header("目标")] [Tooltip("相机跟随的目标物体（例如坦克车体）")] [SerializeField]
    private Transform m_target;

    [Header("距离")] [Tooltip("相机与目标的初始距离")] [SerializeField]
    private float m_distance = 8f;

    [Tooltip("相机与目标的最小距离")] [SerializeField]
    private float m_minDistance = 2f;

    [Tooltip("相机与目标的最大距离")] [SerializeField]
    private float m_maxDistance = 15f;

    [Tooltip("滚轮缩放灵敏度（值越大，每格滚轮的距离变化越明显）")] [SerializeField]
    private float m_scrollSensitivity = 0.5f;

    [Tooltip("是否启用滚轮调节距离")] [SerializeField]
    private bool m_enableScrollZoom = false;

    [Header("高度")] [Tooltip("相机在目标上方的高度偏移")] [SerializeField]
    private float m_height = 8f;

    [Header("旋转")] [Tooltip("鼠标移动灵敏度")] [SerializeField]
    private float m_mouseSensitivity = 2f;

    [Tooltip("俯仰角最小值（向下看的极限，负数表示可以略微向下看）")] [SerializeField]
    private float m_minPitch = -20f;

    [Tooltip("俯仰角最大值（向上看的极限）")] [SerializeField]
    private float m_maxPitch = 60f;

    [Header("平滑")] [Tooltip("位置平滑时间（秒）—— 值越小跟得越紧，值越大越柔和")] [SerializeField]
    private float m_positionSmoothTime = 0.12f;

    [Tooltip("旋转平滑时间（秒）—— 值越小转向越快，值越大越迟钝")] [SerializeField]
    private float m_rotationSmoothTime = 0.12f;

    [Tooltip("距离平滑时间（秒）—— 滚轮缩放时的缓动速度")] [SerializeField]
    private float m_distanceSmoothTime = 0.1f;

    [Header("碰撞检测")] [Tooltip("碰撞检测层级（哪些层的物体会阻挡相机）")] [SerializeField]
    private LayerMask m_collisionMask;

    [Tooltip("碰撞检测球体半径（防止相机镜头贴墙太近）")] [SerializeField]
    private float m_collisionRadius = 2f;

    // ==================== 私有字段（运行时状态） ====================

    // --- 输入目标值（用户鼠标/滚轮操作的直接结果） ---

    /// <summary>偏航角（水平旋转），由鼠标 X 轴控制</summary>
    private float m_yaw;

    /// <summary>俯仰角（垂直旋转），由鼠标 Y 轴控制</summary>
    private float m_pitch;

    // --- 平滑后的值（实际用于相机定位的值） ---

    /// <summary>平滑后的偏航角</summary>
    private float m_smoothYaw;

    /// <summary>平滑后的俯仰角</summary>
    private float m_smoothPitch;

    /// <summary>平滑后的距离</summary>
    private float m_smoothDistance;

    // --- SmoothDamp 所需的速度引用（每次调用会被内部更新） ---

    /// <summary>偏航角平滑速度（SmoothDamp 内部使用）</summary>
    private float m_yawVelocity;

    /// <summary>俯仰角平滑速度（SmoothDamp 内部使用）</summary>
    private float m_pitchVelocity;

    /// <summary>距离平滑速度（SmoothDamp 内部使用）</summary>
    private float m_distanceVelocity;

    /// <summary>位置平滑速度（SmoothDamp 内部使用）</summary>
    private Vector3 m_positionVelocity;

    // --- 编辑器同步 ---
    // （无需额外字段）

    // ==================== 生命周期 ====================

    private void Start()
    {
        // 用相机当前朝向初始化角度，避免第一帧跳转
        Vector3 angles = transform.eulerAngles;
        m_yaw = angles.y;
        m_pitch = angles.x;
        m_smoothYaw = m_yaw;
        m_smoothPitch = m_pitch;
        m_smoothDistance = m_distance;
    }

    private void LateUpdate()
    {
        if (m_target == null)
            return;

        HandleInput();
        UpdateCamera();
    }

    // ==================== 逻辑方法 ====================

    /// <summary>
    /// 处理鼠标和滚轮输入，更新目标角度与目标距离
    /// </summary>
    private void HandleInput()
    {
        if (!m_enableInput) return;

        var mouse = Mouse.current;
        if (mouse == null)
            return;

        // ---- 鼠标移动 → 旋转 ----
        // delta 是鼠标在屏幕上的像素位移量（单位：像素）
        Vector2 delta = mouse.delta.ReadValue();

        // 水平移动 → 偏航角（左右看）
        // 0.1f 是像素到角度的缩放系数，使默认灵敏度手感合理
        m_yaw += delta.x * m_mouseSensitivity * 0.1f;

        // 垂直移动 → 俯仰角（上下看），Y 取反使鼠标上推时相机向上看
        m_pitch -= delta.y * m_mouseSensitivity * 0.1f;
        m_pitch = Mathf.Clamp(m_pitch, m_minPitch, m_maxPitch);

        // ---- 滚轮 → 距离 ----
        if (m_enableScrollZoom)
        {
            // scroll.y 在 Windows 上每格约 120，向下滚为正
            float scroll = mouse.scroll.ReadValue().y;
            if (!Mathf.Approximately(scroll, 0f))
            {
                m_distance -= scroll * m_scrollSensitivity;
                m_distance = Mathf.Clamp(m_distance, m_minDistance, m_maxDistance);
            }
        }
    }

    /// <summary>
    /// 根据平滑后的角度和距离，计算并更新相机位置与朝向
    /// </summary>
    private void UpdateCamera()
    {
        // 第一步：对角度和距离做帧率无关的平滑处理
        m_smoothYaw = Mathf.SmoothDamp(m_smoothYaw, m_yaw, ref m_yawVelocity, m_rotationSmoothTime);
        m_smoothPitch = Mathf.SmoothDamp(m_smoothPitch, m_pitch, ref m_pitchVelocity, m_rotationSmoothTime);
        m_smoothDistance = Mathf.SmoothDamp(m_smoothDistance, m_distance, ref m_distanceVelocity, m_distanceSmoothTime);

        // 第二步：根据平滑后的角度计算相机旋转
        Quaternion rotation = Quaternion.Euler(m_smoothPitch, m_smoothYaw, 0);

        // 第三步：计算目标位置（目标点 + 高度偏移）
        Vector3 targetPosition = m_target.position + Vector3.up * m_height;

        // 第四步：用相机旋转和距离推算理想位置
        Vector3 desiredPosition = targetPosition - rotation * Vector3.forward * m_smoothDistance;

        // 第五步：碰撞检测，如果有遮挡物则把相机拉到遮挡物前方
        desiredPosition = CheckCollision(targetPosition, desiredPosition);

        // 第六步：对位置做帧率无关的平滑移动
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref m_positionVelocity,
            m_positionSmoothTime
        );

        // 第七步：直接朝向目标，旋转不再单独平滑（位置已平滑，朝向自然柔和）
        transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);
    }

    /// <summary>
    /// 从目标点到期望位置做球体射线检测，如果碰到障碍物则将相机位置拉回到碰撞点前方
    /// </summary>
    /// <param name="from">射线起点（目标点）</param>
    /// <param name="to">期望的相机位置</param>
    /// <returns>修正后的相机位置</returns>
    private Vector3 CheckCollision(Vector3 from, Vector3 to)
    {
        Vector3 direction = to - from;
        float length = direction.magnitude;

        // 从目标点向相机方向发射球体射线
        if (Physics.SphereCast(
            from, // 射线起点
            m_collisionRadius, // 球体半径
            direction.normalized, // 射线方向
            out RaycastHit hit, // 碰撞信息
            length, // 射线长度
            m_collisionMask // 检测层级
        ))
        {
            // 命中障碍物：将相机放到碰撞点前方（沿法线推开一个半径的距离）
            return hit.point + hit.normal * m_collisionRadius;
        }

        // 无遮挡：直接使用期望位置
        return to;
    }

    // ==================== 编辑器同步 ====================

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (m_syncInitToTransform)
            ApplyInitToTransform();
    }

    /// <summary>
    /// 将初始化数值（距离、高度等参数）直接同步到Transform（无平滑）
    /// 角度取自当前Transform朝向
    /// </summary>
    private void ApplyInitToTransform()
    {
        if (m_target == null) return;

        Vector3 angles = transform.eulerAngles;
        m_yaw = angles.y;
        m_pitch = angles.x;

        Quaternion rotation = Quaternion.Euler(m_pitch, m_yaw, 0);
        Vector3 targetPosition = m_target.position + Vector3.up * m_height;
        Vector3 desiredPosition = targetPosition - rotation * Vector3.forward * m_distance;
        transform.position = desiredPosition;
        transform.rotation = Quaternion.LookRotation(targetPosition - desiredPosition);
        UnityEditor.EditorUtility.SetDirty(transform);
    }
#endif

    // ==================== 公开接口 ====================

    /// <summary>获取是否接受用户输入</summary>
    public bool IsInputEnabled() => m_enableInput;

    /// <summary>设置是否接受用户输入</summary>
    public void SetEnableInput(bool enable) => m_enableInput = enable;

    /// <summary>获取距离平滑时间（秒）</summary>
    public float GetDistanceSmoothTime() => m_distanceSmoothTime;

    /// <summary>设置距离平滑时间（秒）</summary>
    public void SetDistanceSmoothTime(float time) => m_distanceSmoothTime = time;

    /// <summary>获取碰撞检测球体半径</summary>
    public float GetCollisionRadius() => m_collisionRadius;

    /// <summary>设置碰撞检测球体半径</summary>
    public void SetCollisionRadius(float radius) => m_collisionRadius = radius;

    // /// <summary>获取是否开启编辑器同步</summary>
    // public bool IsSyncInitToTransform() => m_syncInitToTransform;
    //
    // /// <summary>设置是否开启编辑器同步</summary>
    // public void SetSyncInitToTransform(bool sync) => m_syncInitToTransform = sync;
}