using System.Collections;
using UnityEngine;

/// <summary>
/// 怪物脚本
/// 实现 IHittable 接口，可被子弹击中。
/// 受击后扣血，血量 <= 0 时怪物死亡：
/// 1. 禁用碰撞体，不再参与碰撞运算
/// 2. 播放死亡动画
/// 3. 动画播放完毕后沿Y轴向下缓动5个单位
/// 4. 缓动完成后销毁怪物
/// </summary>
public class Monster : MonoBehaviour, IHittable
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

    [Header("生命值")] [Tooltip("最大血量")] [SerializeField]
    private int m_maxHp = 100;

    [Header("死亡配置")] [Tooltip("死亡动画名称（Animator Controller中的状态名）")] [SerializeField]
    private string m_deathAnimName = "died";

    [Tooltip("死亡动画播放完毕后向下缓动的距离（单位）")] [SerializeField]
    private float m_sinkDistance = 5f;

    [Tooltip("向下缓动的持续时间（秒）")] [SerializeField]
    private float m_sinkDuration = 1f;

    // ==================== 私有字段（运行时状态） ====================

    /// <summary>当前血量</summary>
    private int m_currentHp;

    /// <summary>Animator组件引用</summary>
    private Animator m_animator;

    /// <summary>Collider组件引用</summary>
    private Collider m_collider;

    // ==================== 生命周期 ====================

    private void Start()
    {
        m_currentHp = m_maxHp;
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<Collider>();
    }

    // ==================== 受击接口实现 ====================

    /// <summary>
    /// 被子弹击中时调用
    /// 扣除伤害值，血量 <= 0 时死亡
    /// </summary>
    public void OnHit(int damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (m_currentHp <= 0)
            return;

        m_currentHp -= damage;
        Debug.Log($"[Monster] 受到 {damage} 点伤害，剩余血量：{m_currentHp}", this);

        if (m_currentHp <= 0)
        {
            Die();
        }
    }

    // ==================== 私有方法 ====================

    /// <summary>
    /// 怪物死亡流程
    /// 1. 禁用碰撞体 2. 播放死亡动画 3. 等待动画结束 4. 向下缓动 5. 销毁
    /// </summary>
    private void Die()
    {
        Debug.Log("[Monster] 怪物死亡", this);

        // 1. 禁用碰撞体，不再参与碰撞运算
        if (m_collider != null)
            m_collider.enabled = false;

        // 2. 播放死亡动画并等待结束后缓动销毁
        StartCoroutine(PlayDeathAnimationThenSink());
    }

    /// <summary>
    /// 等待死亡动画播放完毕，然后向下缓动并销毁
    /// </summary>
    private IEnumerator PlayDeathAnimationThenSink()
    {
        // 播放死亡动画
        if (m_animator != null && !string.IsNullOrEmpty(m_deathAnimName))
        {
            m_animator.Play(m_deathAnimName);

            // 等待动画进入死亡状态
            yield return null;

            // 等待动画播放完毕
            while (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_deathAnimName) &&
                   m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
        }

        // 3. 沿Y轴向下缓动
        yield return SinkDown();

        // 4. 销毁怪物
        Destroy(gameObject);
    }

    /// <summary>
    /// 沿Y轴向下缓动 m_sinkDistance 个单位，持续 m_sinkDuration 秒
    /// 使用 SmoothStep 缓动曲线
    /// </summary>
    private IEnumerator SinkDown()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos - new Vector3(0f, m_sinkDistance, 0f);
        float elapsed = 0f;

        while (elapsed < m_sinkDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / m_sinkDuration);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
    }

    // ==================== 公开接口 ====================

    /// <summary>当前血量</summary>
    public int CurrentHp => m_currentHp;

    /// <summary>最大血量</summary>
    public int MaxHp => m_maxHp;

    /// <summary>是否已死亡</summary>
    public bool IsDead => m_currentHp <= 0;
}
