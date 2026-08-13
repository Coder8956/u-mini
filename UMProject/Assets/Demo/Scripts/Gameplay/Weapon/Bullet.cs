using UnityEngine;

/// <summary>
/// 子弹运动轨迹计算器
/// 基于物理模型（重力 + 空气阻力）计算并预测子弹飞行轨迹，
/// 子弹正方向（transform.forward）即为运动方向。
/// 支持在Scene窗口绘制预测轨迹用于调试。
/// </summary>
public class Bullet : MonoBehaviour
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

    [Header("运动参数")] [Tooltip("子弹初速度（单位/秒）")] [SerializeField]
    private float m_initialSpeed = 50f;

    [Tooltip("子弹质量（千克）—— 质量越大，空气阻力影响越小")] [SerializeField]
    private float m_mass = 0.2f;

    [Tooltip("是否受重力影响")] [SerializeField]
    private bool m_useGravity = true;

    [Tooltip("空气阻力系数（0表示无空气阻力）")] [SerializeField]
    private float m_dragCoefficient = 0.01f;

    [Header("运动控制")] [Tooltip("是否沿抛物线运动（true=运动，false=停止，再次true=继续运动）")] [SerializeField]
    private bool m_isMoving;

    [Tooltip("沿抛物线运动的速度倍率（支持运行时实时更新）")] [SerializeField]
    private float m_moveSpeed = 0.3f;

    [Header("碰撞配置")] [Tooltip("参与碰撞计算的Layer —— 只有这些Layer上的物体会被子弹检测")] [SerializeField]
    private LayerMask m_collisionLayerMask;

    [Tooltip("子弹造成的伤害值")] [SerializeField]
    private int m_damage = 10;

    [Tooltip("无碰撞时自毁时间（秒）—— 超过此时间未命中则自动销毁，0表示不自毁")] [SerializeField]
    private float m_lifetime = 5f;

    [Header("轨迹调试")] [Tooltip("是否在Scene窗口绘制预测运动轨迹")] [SerializeField]
    private bool m_drawTrajectory = true;

    [Tooltip("轨迹绘制步数（例如500步、1000步）—— 步数越多轨迹越长")] [SerializeField]
    private int m_trajectorySteps = 150;

    [Tooltip("轨迹采样时间步长（秒）—— 值越小轨迹越精确")] [SerializeField]
    private float m_trajectoryStep = 0.02f;

    [Tooltip("预测轨迹颜色")] [SerializeField]
    private Color m_trajectoryColor = Color.magenta;

    [Tooltip("已走过轨迹颜色")] [SerializeField]
    private Color m_traveledColor = Color.blue;

    [Tooltip("抛物线起点标记颜色")] [SerializeField]
    private Color m_startPointColor = Color.yellow;

    // ==================== 私有字段（运行时状态） ====================

    /// <summary>子弹当前速度向量</summary>
    private Vector3 m_velocity;

    /// <summary>抛物线起点位置（debug开启时保留）</summary>
    private Vector3 m_startPosition;

    /// <summary>是否已初始化起点</summary>
    private bool m_startPointInitialized;

    /// <summary>已走过的轨迹采样点（从起点到当前位置）</summary>
    private System.Collections.Generic.List<Vector3> m_traveledPoints = new System.Collections.Generic.List<Vector3>();

    // ==================== 生命周期 ====================

    void Start()
    {
        m_startPosition = transform.position;
        m_velocity = transform.forward * m_initialSpeed;
        m_startPointInitialized = true;
        m_traveledPoints.Add(m_startPosition);

        // 无碰撞时定时自毁
        if (m_lifetime > 0f)
            Destroy(gameObject, m_lifetime);
    }

    void FixedUpdate()
    {
        if (!m_isMoving) return;

        UpdateMovement(Time.fixedDeltaTime);
    }

    // ==================== 碰撞检测 ====================

    /// <summary>
    /// 触发器碰撞检测
    /// 当子弹碰到 m_collisionLayerMask 指定Layer上的物体时：
    /// 1. 调用对方的 IHittable.OnHit 接口
    /// 2. 销毁子弹
    /// 注意：子弹需要挂载 Trigger Collider（且至少一方需要 Rigidbody）
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞对象是否在可碰撞Layer中
        if ((m_collisionLayerMask.value & (1 << other.gameObject.layer)) == 0)
            return;

        // 调用受击接口
        IHittable hittable = other.GetComponent<IHittable>();
        if (hittable != null)
        {
            Vector3 hitDirection = m_velocity.sqrMagnitude > 0.0001f ? m_velocity.normalized : transform.forward;
            hittable.OnHit(m_damage, transform.position, hitDirection);
        }

        // 销毁子弹
        Destroy(gameObject);
    }

    // ==================== 物理计算 ====================

    /// <summary>
    /// 每物理帧更新子弹位置与速度
    /// 加速度 = 重力 + 空气阻力 / 质量
    /// </summary>
    private void UpdateMovement(float deltaTime)
    {
        float scaledDt = deltaTime * m_moveSpeed;
        Vector3 acceleration = ComputeAcceleration(m_velocity);

        m_velocity += acceleration * scaledDt;
        transform.position += m_velocity * scaledDt;

        // 记录已走过的轨迹点
        m_traveledPoints.Add(transform.position);

        // 子弹正方向始终对齐当前速度方向
        if (m_velocity.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(m_velocity);
    }

    /// <summary>
    /// 根据当前速度计算加速度
    /// a = g + F_drag / m，其中 F_drag = -k * v̂ * |v|²
    /// </summary>
    private Vector3 ComputeAcceleration(Vector3 velocity)
    {
        Vector3 acceleration = Vector3.zero;

        if (m_useGravity)
            acceleration += Physics.gravity;

        if (m_dragCoefficient > 0f && velocity.sqrMagnitude > 0f)
        {
            Vector3 dragForce = -m_dragCoefficient * velocity.normalized * velocity.sqrMagnitude;
            acceleration += dragForce / m_mass;
        }

        return acceleration;
    }

    // ==================== 轨迹预测 ====================

    /// <summary>
    /// 从指定起点和速度预计算运动轨迹采样点
    /// 使用固定步长（不受 m_moveSpeed 影响），保证 debug 轨迹长度仅由 m_trajectorySteps 控制
    /// </summary>
    private Vector3[] CalculateTrajectoryPoints(Vector3 startPos, Vector3 startVel)
    {
        int pointCount = Mathf.Max(1, m_trajectorySteps);
        Vector3[] points = new Vector3[pointCount + 1];

        Vector3 pos = startPos;
        Vector3 vel = startVel;
        float dt = m_trajectoryStep;

        points[0] = pos;

        for (int i = 1; i <= pointCount; i++)
        {
            vel += ComputeAcceleration(vel) * dt;
            pos += vel * dt;
            points[i] = pos;
        }

        return points;
    }

    // ==================== Scene调试绘制 ====================

    private void OnDrawGizmos()
    {
        if (!m_drawTrajectory) return;

        // 编辑模式下（未Start）使用当前transform作为起点预测
        Vector3 startPos = m_startPointInitialized ? m_startPosition : transform.position;
        Vector3 startVel = m_startPointInitialized ? transform.forward * m_initialSpeed : transform.forward * m_initialSpeed;

        // ---- 已走过轨迹（起点→当前位置）----
        if (m_startPointInitialized && m_traveledPoints.Count > 1)
        {
            Gizmos.color = m_traveledColor;
            for (int i = 0; i < m_traveledPoints.Count - 1; i++)
                Gizmos.DrawLine(m_traveledPoints[i], m_traveledPoints[i + 1]);
        }

        // ---- 预测未来轨迹（当前位置→未来）----
        Vector3 predictPos = transform.position;
        Vector3 predictVel = m_startPointInitialized ? m_velocity : transform.forward * m_initialSpeed;
        Vector3[] futurePoints = CalculateTrajectoryPoints(predictPos, predictVel);
        Gizmos.color = m_trajectoryColor;
        for (int i = 0; i < futurePoints.Length - 1; i++)
            Gizmos.DrawLine(futurePoints[i], futurePoints[i + 1]);

        // 当前位置标记
        Gizmos.DrawWireSphere(futurePoints[0], 0.2f);

        // 抛物线起点标记
        if (m_startPointInitialized)
        {
            Gizmos.color = m_startPointColor;
            Gizmos.DrawWireSphere(m_startPosition, 0.3f);
        }
    }

    // ==================== 公开接口 ====================

    /// <summary>获取初速度</summary>
    public float GetInitialSpeed() => m_initialSpeed;

    /// <summary>设置初速度</summary>
    public void SetInitialSpeed(float speed) => m_initialSpeed = speed;

    /// <summary>获取子弹质量</summary>
    public float GetMass() => m_mass;

    /// <summary>设置子弹质量</summary>
    public void SetMass(float mass) => m_mass = mass;

    /// <summary>获取是否正在移动</summary>
    public bool IsMoving() => m_isMoving;

    /// <summary>设置是否移动</summary>
    public void SetMoving(bool moving) => m_isMoving = moving;

    /// <summary>获取运动速度倍率</summary>
    public float GetMoveSpeed() => m_moveSpeed;

    /// <summary>设置运动速度倍率</summary>
    public void SetMoveSpeed(float speed) => m_moveSpeed = speed;

    /// <summary>获取预测的轨迹采样点（从当前位置开始）</summary>
    public Vector3[] GetTrajectoryPoints()
    {
        Vector3 startPos = transform.position;
        Vector3 startVel = m_startPointInitialized ? m_velocity : transform.forward * m_initialSpeed;
        return CalculateTrajectoryPoints(startPos, startVel);
    }

    /// <summary>获取碰撞Layer掩码</summary>
    public LayerMask GetCollisionLayerMask() => m_collisionLayerMask;

    /// <summary>设置碰撞Layer掩码</summary>
    public void SetCollisionLayerMask(LayerMask mask) => m_collisionLayerMask = mask;

    /// <summary>获取伤害值</summary>
    public int GetDamage() => m_damage;

    /// <summary>设置伤害值</summary>
    public void SetDamage(int damage) => m_damage = damage;

    /// <summary>获取自毁时间（秒）</summary>
    public float GetLifetime() => m_lifetime;

    /// <summary>设置自毁时间（秒）</summary>
    public void SetLifetime(float time) => m_lifetime = time;
}
