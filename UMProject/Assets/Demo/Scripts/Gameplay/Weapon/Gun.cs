using UnityEngine;

/// <summary>
/// 武器组件
/// 持有炮塔、炮管、射击点、枪口特效的引用，供外部只读访问。
/// </summary>
public class Gun : MonoBehaviour
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

    [Tooltip("炮塔节点（负责水平旋转 Yaw）")]
    [SerializeField]
    private Transform m_turret;

    [Tooltip("炮管节点（负责垂直俯仰 Pitch）")]
    [SerializeField]
    private Transform m_gunBarrel;

    [Tooltip("炮口发射点")]
    [SerializeField]
    private Transform m_shootPoint;

    [Tooltip("枪口特效")]
    [SerializeField]
    private ParticleSystem m_muzzleEffect;

    // ==================== 公开接口（只读） ====================

    /// <summary>获取炮塔节点</summary>
    public Transform GetTurret() => m_turret;

    /// <summary>获取炮管节点</summary>
    public Transform GetGunBarrel() => m_gunBarrel;

    /// <summary>获取炮口发射点</summary>
    public Transform GetShootPoint() => m_shootPoint;

    /// <summary>获取枪口特效</summary>
    public ParticleSystem GetMuzzleEffect() => m_muzzleEffect;
}
