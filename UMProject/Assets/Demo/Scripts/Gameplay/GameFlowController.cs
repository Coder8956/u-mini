using System;
using System.Collections.Generic;
using UMiniFramework.Runtime;
using UnityEngine;

/// <summary>
/// 游戏流程状态
/// </summary>
public enum GameState
{
    /// <summary>空闲 —— 进入场景默认状态，不接受玩家输入</summary>
    Idle,

    /// <summary>游戏中 —— 接受玩家输入</summary>
    Playing,

    /// <summary>暂停 —— 不接受玩家输入</summary>
    Paused,
}

/// <summary>
/// 游戏流程控制器
/// 控制游戏场景的流程状态：
/// 1. 进入场景默认不接受玩家任何操作（Idle 状态）
/// 2. 调用 StartGame() 后接受玩家输入（Playing 状态）
/// 3. 调用 PauseGame() 暂停游戏（Paused 状态，不接受玩家输入）
/// 4. 调用 ResumeGame() 恢复游戏（回到 Playing 状态，接受玩家输入）
/// </summary>
public class GameFlowController : MonoBehaviour
{
    // ==================== 可序列化字段（Inspector 可编辑） ====================

    [Header("引用")]
    [Tooltip("第三人称相机控制器")]
    [SerializeField]
    private TPCameraCtrl m_tpCamera;

    [Tooltip("开火控制器")]
    [SerializeField]
    private GunFireController m_gunFire;

    [Tooltip("大炮瞄准控制器")]
    [SerializeField]
    private GunAimController m_gunAim;

    [Tooltip("武器挂载点（大炮加载后作为其子物体）")]
    [SerializeField]
    private Transform m_weaponPos;

    [Header("流程配置")]
    [Tooltip("进入场景时的初始状态")]
    [SerializeField]
    private GameState m_initialState = GameState.Idle;

    // ==================== 私有字段（运行时状态） ====================

    /// <summary>当前游戏状态</summary>
    private GameState m_gameState;

    /// <summary>场景中所有的怪物（初始化时收集，供后续使用）</summary>
    private List<Monster> m_monsters = new List<Monster>();

    /// <summary>怪物死亡事件侦听器（用于 OnDestroy 时移除）</summary>
    private UMEventListener m_monsterDieListener;

    // ==================== 生命周期 ====================

    private void Awake()
    {
        // 初始化大炮
        InitGun();

        // 初始化子弹
        InitBullet();

        // 收集场景中所有怪物
        InitMonsters();

        // 应用初始状态，根据初始状态配置输入
        m_gameState = m_initialState;
        bool inputAccepted = m_gameState == GameState.Playing;
        SetCameraInput(inputAccepted);
        SetGunFireInput(inputAccepted);
        
        UMOAudio.BGM.Play(DMAudio.BGM_Game);
    }

    private void OnDestroy()
    {
        if (m_monsterDieListener != null)
            UMOEvent.RemoveListener(m_monsterDieListener);

        OnGameStateChanged = null;
    }

    // ==================== 逻辑 ====================

    /// <summary>
    /// 初始化大炮：
    /// 1. 通过 UMOGlobalVal 读取选中的大炮ID
    /// 2. 从 GunTable 加载大炮配置
    /// 3. 根据配置中的 prefabPath 通过 UMORes 加载并实例化大炮预制体
    /// 4. 大炮作为 WeaponPos 的子物体，Transform 归零
    /// 5. 将大炮组件引用绑定到 GunAimController 和 GunFireController
    /// 6. 将大炮 Transform 绑定到第三人称相机
    /// </summary>
    private void InitGun()
    {
        // 检查框架模块是否已初始化
        if (!UMOGlobalVal.IsCreated || !UMOConfig.IsCreated || !UMORes.IsCreated)
        {
            Debug.LogWarning("[GameFlowController] 框架模块未初始化，跳过大炮加载。", this);
            return;
        }

        // 1. 读取选中的大炮ID
        string gunId = UMOGlobalVal.Get<string>(DMGlobalVal.SelectGunID);
        if (string.IsNullOrEmpty(gunId))
        {
            Debug.LogWarning("[GameFlowController] 未设置选中大炮ID。", this);
            return;
        }

        // 2. 加载大炮配置
        GunTable gunTable = UMOConfig.GetTable<GunTable>();
        if (gunTable == null)
        {
            Debug.LogWarning("[GameFlowController] GunTable 未加载。", this);
            return;
        }

        GunData gunData = gunTable.GetDataById(gunId);
        if (gunData == null)
        {
            Debug.LogWarning($"[GameFlowController] 未找到大炮配置：{gunId}", this);
            return;
        }

        // 3. 通过 UMORes 加载并实例化大炮预制体到 WeaponPos 下
        if (m_weaponPos == null)
        {
            Debug.LogWarning("[GameFlowController] WeaponPos 未赋值。", this);
            return;
        }

        GameObject gunGo = UMORes.InstantiateGO(gunData.prefabPath, m_weaponPos);
        if (gunGo == null)
        {
            Debug.LogWarning($"[GameFlowController] 无法加载大炮预制体：{gunData.prefabPath}", this);
            return;
        }

        // 4. Transform 归零
        gunGo.transform.localPosition = Vector3.zero;
        gunGo.transform.localRotation = Quaternion.identity;
        gunGo.transform.localScale = Vector3.one;

        // 5. 绑定大炮组件引用到瞄准与开火控制器
        Gun gun = gunGo.GetComponent<Gun>();
        if (gun == null)
        {
            Debug.LogWarning($"[GameFlowController] 大炮预制体上未找到 Gun 组件：{gunData.prefabPath}", this);
            return;
        }

        if (m_gunAim != null)
        {
            m_gunAim.SetTurret(gun.GetTurret());
            m_gunAim.SetGunBarrel(gun.GetGunBarrel());
            m_gunAim.SetShootPoint(gun.GetShootPoint());
            m_gunAim.SetPitchRange(gunData.minPitch, gunData.maxPitch);
        }

        if (m_gunFire != null)
        {
            m_gunFire.SetReloadTime(gunData.reloadTime);
            m_gunFire.SetMuzzleEffect(gun.GetMuzzleEffect());
        }

        // 6. 将大炮Transform绑定到第三人称相机
        if (m_tpCamera != null)
            m_tpCamera.SetTarget(gunGo.transform);
    }

    /// <summary>
    /// 初始化子弹：
    /// 1. 通过 UMOGlobalVal 读取选中的子弹ID
    /// 2. 从 BulletTable 加载子弹配置
    /// 3. 根据配置中的 prefabPath 通过 UMORes 加载子弹预制体
    /// 4. 将子弹原型和配置参数绑定到 GunFireController
    /// </summary>
    private void InitBullet()
    {
        // 检查框架模块是否已初始化
        if (!UMOGlobalVal.IsCreated || !UMOConfig.IsCreated || !UMORes.IsCreated)
        {
            Debug.LogWarning("[GameFlowController] 框架模块未初始化，跳过子弹加载。", this);
            return;
        }

        // 1. 读取选中的子弹ID
        string bulletId = UMOGlobalVal.Get<string>(DMGlobalVal.SelectBulletID);
        if (string.IsNullOrEmpty(bulletId))
        {
            Debug.LogWarning("[GameFlowController] 未设置选中子弹ID。", this);
            return;
        }

        // 2. 加载子弹配置
        BulletTable bulletTable = UMOConfig.GetTable<BulletTable>();
        if (bulletTable == null)
        {
            Debug.LogWarning("[GameFlowController] BulletTable 未加载。", this);
            return;
        }

        BulletData bulletData = bulletTable.GetDataById(bulletId);
        if (bulletData == null)
        {
            Debug.LogWarning($"[GameFlowController] 未找到子弹配置：{bulletId}", this);
            return;
        }

        // 3. 通过 UMORes 加载子弹预制体
        GameObject bulletPrefab = UMORes.Load<GameObject>(bulletData.prefabPath);
        if (bulletPrefab == null)
        {
            Debug.LogWarning($"[GameFlowController] 无法加载子弹预制体：{bulletData.prefabPath}", this);
            return;
        }

        // 4. 将子弹原型和配置参数绑定到开火控制器
        if (m_gunFire != null)
        {
            m_gunFire.SetBulletPrefab(bulletPrefab);
            m_gunFire.SetBulletParams(bulletData);
            m_gunFire.RefreshBulletPrototype();
        }
    }

    /// <summary>
    /// 收集场景中所有的 Monster 组件到列表中，供后续使用。
    /// 同时注册 DMEventTag.MonsterDie 事件，怪物销毁时从列表移除。
    /// </summary>
    private void InitMonsters()
    {
        m_monsters = new List<Monster>(FindObjectsByType<Monster>(FindObjectsSortMode.None));

        // 注册怪物死亡事件标签并添加侦听器
        UMOEvent.AddEvent(DMEventTag.MonsterDie);
        m_monsterDieListener = new UMEventListener(DMEventTag.MonsterDie, OnMonsterDie);
        UMOEvent.AddListener(m_monsterDieListener);
    }

    /// <summary>
    /// 怪物死亡事件回调：从列表中移除，若列表为空则提示游戏结束
    /// </summary>
    private void OnMonsterDie(UMEventContentBase content)
    {
        if (content is UMEventContent eventContent && eventContent.Content is Monster monster)
            m_monsters.Remove(monster);

        if (m_monsters.Count == 0)
            Debug.Log("[GameFlowController] 所有怪物已被消灭，游戏结束！");
    }

    /// <summary>
    /// 切换游戏状态并触发事件与对应回调
    /// </summary>
    private void SetState(GameState newState)
    {
        if (m_gameState == newState)
            return;

        GameState oldState = m_gameState;
        m_gameState = newState;

        // 根据新状态切换输入
        bool inputAccepted = newState == GameState.Playing;
        SetCameraInput(inputAccepted);
        SetGunFireInput(inputAccepted);

        // 触发状态变更事件
        OnGameStateChanged?.Invoke(newState, oldState);

        // 调用对应状态的回调
        switch (newState)
        {
            case GameState.Idle:
                OnEnterIdle();
                break;
            case GameState.Playing:
                if (oldState == GameState.Idle)
                    OnEnterPlaying();
                else if (oldState == GameState.Paused)
                    OnResumePlaying();
                break;
            case GameState.Paused:
                OnEnterPaused();
                break;
        }
    }

    /// <summary>
    /// 统一控制相机是否接受玩家输入
    /// </summary>
    private void SetCameraInput(bool enabled)
    {
        if (m_tpCamera != null)
            m_tpCamera.SetEnableInput(enabled);
    }

    /// <summary>
    /// 统一控制开火控制器是否接受玩家输入
    /// </summary>
    private void SetGunFireInput(bool enabled)
    {
        if (m_gunFire != null)
            m_gunFire.SetInputEnabled(enabled);
    }

    // ==================== 状态回调（留白，供后续扩展） ====================

    /// <summary>
    /// 进入 Idle 状态时调用（进入场景默认状态）
    /// </summary>
    protected void OnEnterIdle() { }

    /// <summary>
    /// 从 Idle 进入 Playing 状态时调用（开始游戏）
    /// </summary>
    protected void OnEnterPlaying() { }

    /// <summary>
    /// 进入 Paused 状态时调用（暂停游戏）
    /// </summary>
    protected void OnEnterPaused() { }

    /// <summary>
    /// 从 Paused 恢复到 Playing 状态时调用（恢复游戏）
    /// </summary>
    protected void OnResumePlaying() { }

    // ==================== 公开接口 ====================

    /// <summary>当前游戏状态</summary>
    public GameState State => m_gameState;

    /// <summary>是否接受玩家输入（仅 Playing 状态为 true）</summary>
    public bool IsInputAccepted => m_gameState == GameState.Playing;

    /// <summary>获取第三人称相机控制器</summary>
    public TPCameraCtrl TpCamera => m_tpCamera;

    /// <summary>获取开火控制器</summary>
    public GunFireController GunFire => m_gunFire;

    /// <summary>获取大炮瞄准控制器</summary>
    public GunAimController GunAim => m_gunAim;

    /// <summary>获取武器挂载点</summary>
    public Transform WeaponPos => m_weaponPos;

    /// <summary>游戏状态变更事件（参数：newState, oldState）</summary>
    public event Action<GameState, GameState> OnGameStateChanged;

    /// <summary>
    /// 开始游戏 —— 从 Idle 进入 Playing，开始接受玩家输入
    /// </summary>
    public void StartGame()
    {
        if (m_gameState != GameState.Idle)
        {
            Debug.LogWarning($"[GameFlowController] 当前状态为 {m_gameState}，无法开始游戏。", this);
            return;
        }

        SetState(GameState.Playing);
    }

    /// <summary>
    /// 暂停游戏 —— 从 Playing 进入 Paused，停止接受玩家输入
    /// </summary>
    public void PauseGame()
    {
        if (m_gameState != GameState.Playing)
        {
            Debug.LogWarning($"[GameFlowController] 当前状态为 {m_gameState}，无法暂停。", this);
            return;
        }

        SetState(GameState.Paused);
    }

    /// <summary>
    /// 恢复游戏 —— 从 Paused 回到 Playing，重新接受玩家输入
    /// </summary>
    public void ResumeGame()
    {
        if (m_gameState != GameState.Paused)
        {
            Debug.LogWarning($"[GameFlowController] 当前状态为 {m_gameState}，无法恢复。", this);
            return;
        }

        SetState(GameState.Playing);
    }
}
