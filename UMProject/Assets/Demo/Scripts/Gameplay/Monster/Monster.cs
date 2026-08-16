using System.Collections;
using TMPro;
using UMiniFramework.Runtime;
using UnityEngine;

/// <summary>
/// 怪物脚本
/// 实现 IHittable 接口，可被子弹击中。
/// 通过配置表ID读取怪物配置，初始化血量和名称。
/// 受击后扣血，血量 <= 0 时怪物死亡：
/// 1. 禁用碰撞体，不再参与碰撞运算
/// 2. 播放死亡动画
/// 3. 动画播放完毕后沿Y轴向下缓动5个单位
/// 4. 缓动完成后销毁怪物
/// </summary>
public class Monster : MonoBehaviour, IHittable
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

    [Header("配置")] [Tooltip("怪物配置表ID（对应MonsterTable中的id）")] [SerializeField]
    private string m_monsterId;

    [Header("显示")] [Tooltip("用于显示怪物名字的TMP文本（留空则不显示）")] [SerializeField]
    private TMP_Text m_nameText;

    [Tooltip("HPState子物体上的TMP文本，用于显示血量状态（留空则不显示）")] [SerializeField]
    private TMP_Text m_hpText;

    [Tooltip("锁定提示渲染器（被相机射线命中时变红，留空则不显示锁定状态）")] [SerializeField]
    private Renderer m_lockedTipRenderer;

    [Header("死亡配置")] [Tooltip("死亡动画名称（Animator Controller中的状态名）")] [SerializeField]
    private string m_deathAnimName = "died";

    [Tooltip("死亡动画播放完毕后向下缓动的距离（单位）")] [SerializeField]
    private float m_sinkDistance = 5f;

    [Tooltip("向下缓动的持续时间（秒）")] [SerializeField]
    private float m_sinkDuration = 1f;

    // ==================== 私有字段（运行时状态） ====================

    /// <summary>怪物配置数据</summary>
    private MonsterData m_monsterData;

    /// <summary>最大血量（从配置读取）</summary>
    private int m_maxHp;

    /// <summary>当前血量</summary>
    private int m_currentHp;

    /// <summary>Animator组件引用</summary>
    private Animator m_animator;

    /// <summary>Collider组件引用</summary>
    private Collider m_collider;

    /// <summary>LockedTip初始颜色</summary>
    private Color m_lockedTipInitialColor;

    /// <summary>LockedTip材质颜色属性名（URP shader使用_BaseColor）</summary>
    private const string k_ColorProp = "_BaseColor";

    /// <summary>当前是否被相机射线锁定</summary>
    private bool m_isLocked;

    // ==================== 生命周期 ====================

    private void Start()
    {
        LoadMonsterConfig();

        m_currentHp = m_maxHp;
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<Collider>();

        if (m_lockedTipRenderer != null)
            m_lockedTipInitialColor = m_lockedTipRenderer.material.GetColor(k_ColorProp);
    }

    private void LateUpdate()
    {
        UpdateHpText();
    }

    private void OnDestroy()
    {
        UMOEvent.Dispatch(DMEventTag.MonsterDie, new UMEventContent(this));
    }

    // ==================== 配置加载 ====================

    /// <summary>
    /// 通过怪物ID读取配置表，初始化血量和名称
    /// </summary>
    private void LoadMonsterConfig()
    {
        if (string.IsNullOrEmpty(m_monsterId))
        {
            Debug.LogWarning("[Monster] 未设置怪物配置ID。", this);
            return;
        }

        MonsterTable monsterTable = UMOConfig.GetTable<MonsterTable>();
        if (monsterTable == null)
        {
            Debug.LogWarning("[Monster] MonsterTable 未加载。", this);
            return;
        }

        m_monsterData = monsterTable.GetDataById(m_monsterId);
        if (m_monsterData == null)
        {
            Debug.LogWarning($"[Monster] 未找到怪物配置：{m_monsterId}", this);
            return;
        }

        // 从配置读取最大血量
        m_maxHp = m_monsterData.HP;

        // 显示怪物名字
        if (m_nameText != null)
            m_nameText.SetText(m_monsterData.name);
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
    /// 更新HPState子物体上的血量显示，格式为 最大血量/剩余血量
    /// </summary>
    private void UpdateHpText()
    {
        if (m_hpText != null)
            m_hpText.SetText($"{m_maxHp}/{m_currentHp}");
    }

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

    /// <summary>怪物配置ID</summary>
    public string MonsterId => m_monsterId;

    /// <summary>怪物名称</summary>
    public string MonsterName => m_monsterData?.name;

    /// <summary>当前血量</summary>
    public int CurrentHp => m_currentHp;

    /// <summary>最大血量</summary>
    public int MaxHp => m_maxHp;

    /// <summary>是否已死亡</summary>
    public bool IsDead => m_currentHp <= 0;

    /// <summary>
    /// 设置锁定状态，命中时LockedTip变红，未命中恢复初始颜色
    /// </summary>
    public void SetLocked(bool locked)
    {
        if (m_isLocked == locked)
            return;

        m_isLocked = locked;

        if (m_lockedTipRenderer != null)
            m_lockedTipRenderer.material.SetColor(k_ColorProp, locked ? Color.red : m_lockedTipInitialColor);
    }
}
