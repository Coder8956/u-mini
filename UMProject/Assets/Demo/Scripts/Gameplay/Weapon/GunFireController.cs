using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 开火控制器
/// 控制武器开火流程：
/// 1. 持有GunAimController引用，从开火点发射子弹
/// 2. 持有子弹游戏物体原型（运行时由GameFlowController从配置表加载并设置）
/// 3. 点击鼠标左键开火
/// 4. 开火后在开火点实例化子弹，子弹世界位置和旋转等于开火点
/// 5. 将子弹配置参数应用到实例化的子弹上
/// 6. 启动子弹运动
/// 7. 支持装弹时间（秒）—— 两次开火之间的最小间隔
/// 8. 支持控制是否接受玩家输入
/// 9. 支持显示子弹原型（生成失活子物体，便于开发者查看子弹参数）
/// </summary>
public class GunFireController : MonoBehaviour
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

    [Header("引用")] [Tooltip("大炮瞄准控制器（提供开火点）")] [SerializeField]
    private GunAimController m_gunAimController;

    [Header("开火参数")] [Tooltip("装弹时间（秒）—— 两次开火之间的最小间隔")] [SerializeField]
    private float m_reloadTime = 1f;

    [Tooltip("是否接受玩家输入")] [SerializeField]
    private bool m_inputEnabled = true;

    [Header("开火特效")] [Tooltip("开火粒子特效（每次开火播放一次）")] [SerializeField]
    private ParticleSystem m_muzzleEffect;

    [Header("调试")] [Tooltip("是否显示子弹原型（生成一个失活的子弹子物体，便于开发者查看子弹参数）")] [SerializeField]
    private bool m_showBulletPrototype;

    // ==================== 私有字段（运行时状态） ====================

    /// <summary>子弹游戏物体原型（由GameFlowController从配置表加载后设置）</summary>
    private GameObject m_bulletPrefab;

    /// <summary>子弹配置数据（由GameFlowController从配置表读取后设置）</summary>
    private BulletData m_bulletData;

    /// <summary>子弹原型展示对象（失活子物体，仅供开发者查看）</summary>
    private GameObject m_bulletPrototypeDisplay;

    /// <summary>当前装弹计时器（秒）—— 剩余装弹时间，为0时可开火</summary>
    private float m_reloadTimer;

    // ==================== 生命周期 ====================

    private void Update()
    {
        // 装弹计时器递减
        if (m_reloadTimer > 0f)
            m_reloadTimer -= Time.deltaTime;

        // 检查开火输入
        if (m_inputEnabled && m_reloadTimer <= 0f && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Fire();
        }
    }

    private void OnDestroy()
    {
        if (m_bulletPrototypeDisplay != null)
            Destroy(m_bulletPrototypeDisplay);
    }

    // ==================== 开火逻辑 ====================

    /// <summary>
    /// 执行开火流程：
    /// 1. 从GunAimController获取开火点
    /// 2. 从子弹原型实例化子弹
    /// 3. 在开火点实例化子弹，世界位置和旋转等于开火点
    /// 4. 应用子弹配置参数
    /// 5. 启动子弹运动
    /// 6. 重置装弹计时器
    /// </summary>
    private void Fire()
    {
        if (m_gunAimController == null)
        {
            Debug.LogWarning("[GunFireController] GunAimController 未赋值，无法开火。", this);
            return;
        }

        Transform shootPoint = m_gunAimController.GetShootPoint();
        if (shootPoint == null)
        {
            Debug.LogWarning("[GunFireController] 开火点未设置，无法开火。", this);
            return;
        }

        if (m_bulletPrefab == null)
        {
            Debug.LogWarning("[GunFireController] 子弹原型未设置，无法开火。", this);
            return;
        }

        // 在开火点实例化子弹，世界位置和旋转等于开火点
        GameObject bulletGo = Instantiate(m_bulletPrefab, shootPoint.position, shootPoint.rotation);

        // 播放开火特效
        if (m_muzzleEffect != null)
            m_muzzleEffect.Play();

        // 应用子弹配置参数并启动子弹运动
        Bullet bullet = bulletGo.GetComponent<Bullet>();
        if (bullet != null)
        {
            ApplyBulletParams(bulletGo);
            bullet.SetMoving(true);
        }

        // 重置装弹计时器
        m_reloadTimer = m_reloadTime;
    }

    /// <summary>
    /// 创建或销毁子弹原型展示对象
    /// 当 m_showBulletPrototype 为 true 时，生成一个失活的子弹子物体供开发者查看参数
    /// </summary>
    private void UpdateBulletPrototypeDisplay()
    {
        // 销毁已有的展示对象
        if (m_bulletPrototypeDisplay != null)
        {
            Destroy(m_bulletPrototypeDisplay);
            m_bulletPrototypeDisplay = null;
        }

        // 如果不需要显示或没有原型，直接返回
        if (!m_showBulletPrototype || m_bulletPrefab == null)
            return;

        // 生成失活的子弹子物体
        m_bulletPrototypeDisplay = Instantiate(m_bulletPrefab, transform);
        m_bulletPrototypeDisplay.name = "[Prototype] " + m_bulletPrefab.name;
        m_bulletPrototypeDisplay.SetActive(false);

        // 将配置参数应用到原型的 Bullet 组件，便于开发者直接在 Inspector 中查看
        ApplyBulletParams(m_bulletPrototypeDisplay);
    }

    /// <summary>
    /// 将 m_bulletData 中的配置参数应用到指定子弹游戏物体的 Bullet 组件上
    /// </summary>
    private void ApplyBulletParams(GameObject bulletGo)
    {
        if (m_bulletData == null)
            return;

        Bullet bullet = bulletGo.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDamage(m_bulletData.damage);
            bullet.SetMass(m_bulletData.mass);
            bullet.SetInitialSpeed(m_bulletData.initSpeed);
            bullet.SetMoveSpeed(m_bulletData.moveSpeed);
        }
    }

    // ==================== 公开接口 ====================

    /// <summary>获取是否接受玩家输入</summary>
    public bool IsInputEnabled() => m_inputEnabled;

    /// <summary>设置是否接受玩家输入</summary>
    public void SetInputEnabled(bool enabled) => m_inputEnabled = enabled;

    /// <summary>获取装弹时间（秒）</summary>
    public float GetReloadTime() => m_reloadTime;

    /// <summary>设置装弹时间（秒）</summary>
    public void SetReloadTime(float time) => m_reloadTime = time;

    /// <summary>设置开火粒子特效</summary>
    public void SetMuzzleEffect(ParticleSystem effect) => m_muzzleEffect = effect;

    /// <summary>
    /// 设置子弹游戏物体原型
    /// 由 GameFlowController 从子弹配置表加载后调用
    /// </summary>
    public void SetBulletPrefab(GameObject prefab)
    {
        m_bulletPrefab = prefab;
    }

    /// <summary>
    /// 设置子弹配置参数
    /// 由 GameFlowController 从子弹配置表读取后调用
    /// </summary>
    public void SetBulletParams(BulletData data)
    {
        m_bulletData = data;
    }

    /// <summary>
    /// 刷新子弹原型展示
    /// 在 SetBulletPrefab 和 SetBulletParams 都调用完毕后调用一次
    /// </summary>
    public void RefreshBulletPrototype()
    {
        UpdateBulletPrototypeDisplay();
    }

    /// <summary>当前是否正在装弹（无法开火）</summary>
    public bool IsReloading() => m_reloadTimer > 0f;

    /// <summary>获取剩余装弹时间（秒）</summary>
    public float GetRemainingReloadTime() => Mathf.Max(0f, m_reloadTimer);
}
