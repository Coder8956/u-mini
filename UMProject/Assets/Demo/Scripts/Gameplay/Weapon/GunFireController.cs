using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 开火控制器
/// 控制武器开火流程：
/// 1. 持有GunAimController引用，从开火点发射子弹
/// 2. 通过Resources路径加载子弹预制体
/// 3. 点击鼠标左键开火
/// 4. 开火后在开火点实例化子弹，子弹世界位置和旋转等于开火点
/// 5. 启动子弹运动
/// 6. 支持装弹时间（秒）—— 两次开火之间的最小间隔
/// 7. 支持控制是否接受玩家输入
/// </summary>
public class GunFireController : MonoBehaviour
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

    [Header("引用")] [Tooltip("大炮瞄准控制器（提供开火点）")] [SerializeField]
    private GunAimController m_gunAimController;

    [Header("子弹配置")] [Tooltip("子弹预制体在Resources文件夹下的路径（例如 \"Prefabs/Bullet\"）")] [SerializeField]
    private string m_bulletResourcesPath = "Prefabs/Bullet";

    [Header("开火参数")] [Tooltip("装弹时间（秒）—— 两次开火之间的最小间隔")] [SerializeField]
    private float m_reloadTime = 1f;

    [Tooltip("是否接受玩家输入")] [SerializeField]
    private bool m_inputEnabled = true;

    // ==================== 私有字段（运行时状态） ====================

    /// <summary>当前装弹计时器（秒）—— 剩余装弹时间，为0时可开火</summary>
    private float m_reloadTimer;

    // ==================== 生命周期 ====================

    void Update()
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

    // ==================== 开火逻辑 ====================

    /// <summary>
    /// 执行开火流程：
    /// 1. 从GunAimController获取开火点
    /// 2. 从Resources加载子弹预制体
    /// 3. 在开火点实例化子弹，世界位置和旋转等于开火点
    /// 4. 启动子弹运动
    /// 5. 重置装弹计时器
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

        // 从Resources加载子弹预制体
        GameObject bulletPrefab = Resources.Load<GameObject>(m_bulletResourcesPath);
        if (bulletPrefab == null)
        {
            Debug.LogWarning($"[GunFireController] 无法从Resources路径加载子弹预制体：{m_bulletResourcesPath}", this);
            return;
        }

        // 在开火点实例化子弹，世界位置和旋转等于开火点
        GameObject bulletGo = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        // 启动子弹运动
        Bullet bullet = bulletGo.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetMoving(true);
        }

        // 重置装弹计时器
        m_reloadTimer = m_reloadTime;
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

    /// <summary>当前是否正在装弹（无法开火）</summary>
    public bool IsReloading() => m_reloadTimer > 0f;

    /// <summary>获取剩余装弹时间（秒）</summary>
    public float GetRemainingReloadTime() => Mathf.Max(0f, m_reloadTimer);
}
