using UnityEngine;

/// <summary>
/// 怪物脚本
/// 实现 IHittable 接口，可被子弹击中。
/// 受击后扣血，血量 <= 0 时怪物死亡（销毁）。
/// </summary>
public class Monster : MonoBehaviour, IHittable
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

    [Header("生命值")] [Tooltip("最大血量")] [SerializeField]
    private int m_maxHp = 100;

    // ==================== 私有字段（运行时状态） ====================

    /// <summary>当前血量</summary>
    private int m_currentHp;

    // ==================== 生命周期 ====================

    void Start()
    {
        m_currentHp = m_maxHp;
    }

    // ==================== 受击接口实现 ====================

    /// <summary>
    /// 被子弹击中时调用
    /// 扣除伤害值，血量 <= 0 时死亡
    /// </summary>
    public void OnHit(int damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        m_currentHp -= damage;
        Debug.Log($"[Monster] 受到 {damage} 点伤害，剩余血量：{m_currentHp}", this);

        if (m_currentHp <= 0)
        {
            Die();
        }
    }

    // ==================== 私有方法 ====================

    /// <summary>
    /// 怪物死亡 —— 销毁游戏对象
    /// </summary>
    private void Die()
    {
        Debug.Log("[Monster] 怪物死亡", this);
        Destroy(gameObject);
    }

    // ==================== 公开接口 ====================

    /// <summary>当前血量</summary>
    public int CurrentHp => m_currentHp;

    /// <summary>最大血量</summary>
    public int MaxHp => m_maxHp;

    /// <summary>是否已死亡</summary>
    public bool IsDead => m_currentHp <= 0;
}
