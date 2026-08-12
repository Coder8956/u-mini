using UnityEngine;

/// <summary>
/// 大炮瞄准控制器
/// 完整实现大炮瞄准流程：
/// 1. 从相机正方向发射射线，获取世界瞄准点（碰撞点或射线端点）
/// 2. 控制炮塔水平旋转(Yaw)朝向瞄准点
/// 3. 控制炮管垂直俯仰(Pitch)朝向瞄准点，俯仰角度受参数限制
/// 4. 从炮管正方向发射射线，获取最终射击目标点（碰撞点或世界瞄准点）
/// </summary>
public class GunAimController : MonoBehaviour
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

    [Header("引用")] [Tooltip("用于发射瞄准射线的相机（留空自动使用主相机）")] [SerializeField]
    private Camera m_aimCamera;

    [Tooltip("炮塔节点（负责水平旋转 Yaw）")] [SerializeField]
    private Transform m_turret;

    [Tooltip("炮管节点（负责垂直俯仰 Pitch）")] [SerializeField]
    private Transform m_gunBarrel;

    [Tooltip("炮口发射点（留空则使用炮管位置）")] [SerializeField]
    private Transform m_shootPoint;

    [Header("相机射线参数")] [Tooltip("相机射线最大长度（单位）")] [SerializeField]
    private float m_cameraRayDistance = 200f;

    [Tooltip("瞄准射线碰撞层级")] [SerializeField]
    private LayerMask m_aimLayerMask = ~0;

    [Header("俯仰限制")] [Tooltip("炮管最大仰角（向上，度）")] [SerializeField]
    private float m_maxPitch = 80f;

    [Tooltip("炮管最大俯角（向下，度）")] [SerializeField]
    private float m_minPitch = -10f;

    [Header("旋转平滑")] [Tooltip("炮塔旋转平滑时间（秒）—— 值越小转得越快")] [SerializeField]
    private float m_turretSmoothTime = 0.1f;

    [Tooltip("炮管俯仰平滑时间（秒）—— 值越小响应越快")] [SerializeField]
    private float m_gunSmoothTime = 0.1f;

    [Header("Scene射线调试")] [Tooltip("是否在Scene窗口绘制射线参考线")] [SerializeField]
    private bool m_drawDebugRays = true;

    [Tooltip("相机瞄准射线颜色")] [SerializeField]
    private Color m_cameraRayColor = Color.cyan;

    [Tooltip("炮管射击射线颜色")] [SerializeField]
    private Color m_gunRayColor = Color.red;

    // ==================== 私有字段（运行时状态） ====================

    /// <summary>当前炮塔偏航角（度）</summary>
    private float m_currentYaw;

    /// <summary>当前炮管俯仰角（度，正值表示仰角）</summary>
    private float m_currentPitch;

    /// <summary>偏航角平滑速度（SmoothDamp 内部使用）</summary>
    private float m_yawVelocity;

    /// <summary>俯仰角平滑速度（SmoothDamp 内部使用）</summary>
    private float m_pitchVelocity;

    /// <summary>世界瞄准点（由相机射线计算得出）</summary>
    private Vector3 m_worldAimPoint;

    /// <summary>最终射击目标点（由炮管射线计算得出）</summary>
    private Vector3 m_shootTargetPoint;

    // ==================== 生命周期 ====================

    void Start()
    {
        if (m_aimCamera == null)
            m_aimCamera = Camera.main;

        // 从炮塔当前朝向初始化偏航角，避免第一帧跳转
        if (m_turret != null)
            m_currentYaw = m_turret.eulerAngles.y;

        // 从炮管当前局部旋转初始化俯仰角
        if (m_gunBarrel != null)
        {
            float localX = m_gunBarrel.localEulerAngles.x;
            if (localX > 180f) localX -= 360f;
            m_currentPitch = -localX;
        }
    }

    void Update()
    {
        UpdateAim();
    }

    // ==================== 瞄准流程 ====================

    /// <summary>
    /// 每帧执行完整瞄准流程：获取世界瞄准点 → 炮塔Yaw → 炮管Pitch → 获取射击目标点
    /// </summary>
    private void UpdateAim()
    {
        // Step 1: 从相机正方向发射射线，获取世界瞄准点
        m_worldAimPoint = GetWorldAimPoint();

        // Step 2: 控制炮塔水平旋转(Yaw)朝向瞄准点
        UpdateTurretYaw(m_worldAimPoint);

        // Step 3: 控制炮管垂直俯仰(Pitch)朝向瞄准点
        UpdateGunPitch(m_worldAimPoint);

        // Step 4: 从炮管正方向发射射线，获取射击目标点
        m_shootTargetPoint = GetShootTargetPoint();
    }

    // ==================== 逻辑方法 ====================

    /// <summary>
    /// 从相机正方向发射射线，获取世界瞄准点
    /// 如果射线命中碰撞体，使用碰撞点；否则使用射线结束端点
    /// </summary>
    private Vector3 GetWorldAimPoint()
    {
        if (m_aimCamera == null)
            return transform.position;

        Ray ray = new Ray(m_aimCamera.transform.position, m_aimCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, m_cameraRayDistance, m_aimLayerMask))
        {
            return hit.point;
        }

        return ray.GetPoint(m_cameraRayDistance);
    }

    /// <summary>
    /// 控制炮塔水平旋转（Yaw），使其朝向世界瞄准点
    /// 仅旋转Y轴，忽略高度差
    /// </summary>
    private void UpdateTurretYaw(Vector3 aimPoint)
    {
        if (m_turret == null) return;

        Vector3 direction = aimPoint - m_turret.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        float targetYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        m_currentYaw = Mathf.SmoothDampAngle(m_currentYaw, targetYaw, ref m_yawVelocity, m_turretSmoothTime);
        m_turret.rotation = Quaternion.Euler(0f, m_currentYaw, 0f);
    }

    /// <summary>
    /// 控制炮管垂直俯仰（Pitch），使其朝向世界瞄准点
    /// 俯仰角度受 m_minPitch / m_maxPitch 限制
    /// </summary>
    private void UpdateGunPitch(Vector3 aimPoint)
    {
        if (m_gunBarrel == null) return;

        Vector3 direction = aimPoint - m_gunBarrel.position;

        // 计算水平距离和高度差
        Vector3 flat = direction;
        flat.y = 0f;
        float horizontalDist = flat.magnitude;

        if (horizontalDist < 0.001f) return;

        // 俯仰角：正值表示仰角（目标在上方），负值表示俯角（目标在下方）
        float targetPitch = Mathf.Atan2(direction.y, horizontalDist) * Mathf.Rad2Deg;
        targetPitch = Mathf.Clamp(targetPitch, m_minPitch, m_maxPitch);

        m_currentPitch = Mathf.SmoothDampAngle(m_currentPitch, targetPitch, ref m_pitchVelocity, m_gunSmoothTime);

        // Unity中正X旋转使炮管朝下，因此取负值
        m_gunBarrel.localRotation = Quaternion.Euler(-m_currentPitch, 0f, 0f);
    }

    /// <summary>
    /// 从炮管正方向发射射线，获取射击目标点
    /// 如果射线命中碰撞体，使用碰撞点；否则使用世界瞄准点
    /// </summary>
    private Vector3 GetShootTargetPoint()
    {
        if (m_gunBarrel == null) return m_worldAimPoint;

        Vector3 origin = m_shootPoint != null ? m_shootPoint.position : m_gunBarrel.position;
        Vector3 direction = m_gunBarrel.forward;

        if (Physics.Raycast(new Ray(origin, direction), out RaycastHit hit, m_cameraRayDistance, m_aimLayerMask))
        {
            return hit.point;
        }

        return m_worldAimPoint;
    }

    // ==================== Scene调试绘制 ====================

    private void OnDrawGizmos()
    {
        if (!m_drawDebugRays) return;

        // 相机瞄准射线：从相机位置沿正前方绘制
        if (m_aimCamera != null)
        {
            Vector3 camPos = m_aimCamera.transform.position;
            Vector3 camDir = m_aimCamera.transform.forward;
            Vector3 camEnd = camPos + camDir * m_cameraRayDistance;

            Gizmos.color = m_cameraRayColor;
            Gizmos.DrawLine(camPos, camEnd);
            Gizmos.DrawSphere(m_worldAimPoint, 0.3f);
        }

        // 炮管射击射线：从炮口沿炮管正前方绘制
        if (m_gunBarrel != null)
        {
            Vector3 origin = m_shootPoint != null ? m_shootPoint.position : m_gunBarrel.position;
            Vector3 dir = m_gunBarrel.forward;
            Vector3 end = origin + dir * m_cameraRayDistance;

            Gizmos.color = m_gunRayColor;
            Gizmos.DrawLine(origin, end);
            Gizmos.DrawWireSphere(m_shootTargetPoint, 0.5f);
        }
    }

    // ==================== 公开接口 ====================

    /// <summary>获取当前射击目标点（供开火逻辑调用）</summary>
    public Vector3 GetShootTarget() => m_shootTargetPoint;

    /// <summary>获取当前世界瞄准点</summary>
    public Vector3 GetWorldAimPointValue() => m_worldAimPoint;
}
