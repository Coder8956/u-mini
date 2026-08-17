using UMiniFramework.Runtime;
using UnityEngine;

/// <summary>
/// Demo 全局实体单例
/// 不可销毁，跨场景持久存在
/// 仅通过静态成员使用
/// </summary>
public class DemoEntity : MonoBehaviour
{
    // ==================== 私有字段（运行时状态） ====================

    private static DemoEntity Instance { get; set; }

    /// <summary>是否已创建</summary>
    public static bool IsCreated => Instance != null;

    private DemoData m_data;

    // ==================== 生命周期 ====================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ReadDemoData();
    }

    private void ReadDemoData()
    {
        m_data = UMOPersist.Read(DemoData.FilePath, new DemoData());

        UMOGlobalVal.Set(DMGlobalVal.SelectGunID, m_data.GunID);
        UMOGlobalVal.Set(DMGlobalVal.SelectBulletID, m_data.BulletID);
        if (UMOConfig.Local != null)
        {
            UMOConfig.Local.SwitchByCode(m_data.LangCode);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    // ==================== 公开接口 ====================

    /// <summary>
    /// 创建全局唯一实例
    /// 已存在时直接返回
    /// </summary>
    public static void Create()
    {
        if (Instance != null)
            return;

        var go = new GameObject(nameof(DemoEntity));
        go.AddComponent<DemoEntity>();
    }

    private void OnApplicationQuit()
    {
        if (UMOConfig.Local != null)
        {
            m_data.LangCode = UMOConfig.Local.CurtCode;
        }

        m_data.GunID = UMOGlobalVal.Get<string>(DMGlobalVal.SelectGunID);
        m_data.BulletID = UMOGlobalVal.Get<string>(DMGlobalVal.SelectBulletID);

        UMOPersist.Save(DemoData.FilePath, m_data);
    }
}