using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 怪物血条控制器
/// 通过前景条的 Fill Amount 同步 Monster 当前血量百分比。
/// 血条由固定长度背景条和表示剩余血量的前景条组成。
/// </summary>
public class MonsterHealthBar : MonoBehaviour
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

    [Header("引用")] [Tooltip("怪物脚本（留空则自动查找）")] [SerializeField]
    private Monster m_monster;

    [Tooltip("背景条Image（固定长度）")] [SerializeField]
    private Image m_backgroundImage;

    [Tooltip("前景条Image（通过Fill Amount体现剩余血量）")] [SerializeField]
    private Image m_foregroundImage;

    [Header("颜色")] [Tooltip("背景条颜色")] [SerializeField]
    private Color m_backgroundColor = Color.red;

    [Tooltip("前景条颜色")] [SerializeField]
    private Color m_foregroundColor = Color.green;

    // ==================== 生命周期 ====================

    private void Awake()
    {
        if (m_monster == null)
            m_monster = GetComponentInParent<Monster>();

        if (m_backgroundImage != null)
            m_backgroundImage.color = m_backgroundColor;

        if (m_foregroundImage != null)
        {
            m_foregroundImage.color = m_foregroundColor;
            m_foregroundImage.type = Image.Type.Filled;
            m_foregroundImage.fillMethod = Image.FillMethod.Horizontal;
            m_foregroundImage.fillOrigin = 0;
        }
    }

    private void LateUpdate()
    {
        // 血条正方向始终正对相机
        Camera cam = Camera.main;
        if (cam != null)
            transform.rotation = cam.transform.rotation;

        if (m_monster == null)
            return;

        // 怪物死亡时隐藏血条
        if (m_monster.IsDead)
        {
            gameObject.SetActive(false);
            return;
        }

        if (m_foregroundImage == null)
            return;

        int maxHp = m_monster.MaxHp;
        if (maxHp <= 0)
            return;

        m_foregroundImage.fillAmount = (float)m_monster.CurrentHp / maxHp;
    }

    // ==================== 公开接口 ====================

    /// <summary>获取怪物脚本</summary>
    public Monster GetMonster() => m_monster;

    /// <summary>设置怪物脚本</summary>
    public void SetMonster(Monster monster) => m_monster = monster;

    /// <summary>获取背景条Image</summary>
    public Image GetBackgroundImage() => m_backgroundImage;

    /// <summary>设置背景条Image</summary>
    public void SetBackgroundImage(Image image) => m_backgroundImage = image;

    /// <summary>获取前景条Image</summary>
    public Image GetForegroundImage() => m_foregroundImage;

    /// <summary>设置前景条Image</summary>
    public void SetForegroundImage(Image image) => m_foregroundImage = image;

    /// <summary>获取背景条颜色</summary>
    public Color GetBackgroundColor() => m_backgroundColor;

    /// <summary>设置背景条颜色</summary>
    public void SetBackgroundColor(Color color) => m_backgroundColor = color;

    /// <summary>获取前景条颜色</summary>
    public Color GetForegroundColor() => m_foregroundColor;

    /// <summary>设置前景条颜色</summary>
    public void SetForegroundColor(Color color) => m_foregroundColor = color;
}
