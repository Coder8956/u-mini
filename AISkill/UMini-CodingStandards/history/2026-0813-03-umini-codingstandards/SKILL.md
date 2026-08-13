---
name: umini-codingstandards
description: UMiniFramework C# 编码规范。在编写或修改 UMiniFramework 相关 C# 脚本时自动激活，确保 UM 前缀命名、UMMonoSingleton 单例模式、模块静态 API 封装、UI 面板 UMUIPanelBase + UMUIPanelCfg 特性驱动、配置表/事件/持久化基类继承、Inspector 字段 [SerializeField] private、成员变量 m_ 前缀、分区注释分隔符等规范。触发于"写脚本"、"创建脚本"、"代码规范"、"新建模块"、"新建 UI 面板"、"UMini"、"UM"等。
---

# UMiniFramework 编码规范

## 框架根目录

`Assets/UMiniFramework`

## 目录结构

```
Assets/UMiniFramework/
├── Runtime/
│   ├── Core/           # 核心基类（UMMonoSingleton）
│   ├── Launcher/       # 框架入口（UMLauncher）
│   ├── Module/         # 功能模块
│   │   ├── Config/     # 配置表系统
│   │   ├── Event/      # 事件系统
│   │   ├── Persist/    # 数据持久化
│   │   ├── Res/        # 资源加载
│   │   ├── Scene/      # 场景管理
│   │   └── UI/         # UI 面板系统
│   └── UMiniFramework.Runtime.asmdef
├── Editor/
│   ├── Inspector/      # 自定义 Inspector
│   ├── Windows/        # EditorWindow 工具
│   └── Plugins/        # Editor 侧第三方库
└── Plugins/            # Runtime 侧第三方库（如 Newtonsoft.Json）
```

## 命名空间

| 目录 | 命名空间 |
|------|----------|
| Runtime | `UMiniFramework.Runtime` |
| Editor | `UMiniFramework.Editor` |

## 命名规范

### 类

- `PascalCase`，统一添加 `UM` 前缀
- 模块管理器（`UM Module`）：`UMO` + 功能名（`UMORes`、`UMOUI`、`UMOEvent`、`UMOConfig`、`UMOPersist`、`UMOScene`）
- 工具类：`UM` + 功能名 + `Utils`（`UMUIUtils`、`UMConfigUtils`）
- 泛型类：`UM` + 功能名 + `<K, V>`（`UMCfgKV<K, V>`）
- 普通业务脚本：`PascalCase`，统一添加 `UM` 前缀

### 接口

- `IUM` + 能力名词/形容词（`IUMLangTable`）
- 优先"能做什么"语义

### 基类

命名模式 `UM` + 功能名 + `Base` （`UMUIPanelBase`）

### 枚举

- `UM` 前缀 + `PascalCase`（`UMListenType`）
- 枚举值 `PascalCase`，带 `/// <summary>` XML 注释

### 结构体

- `PascalCase` 统一添加 `UM` 前缀（如 `UMLangOption`）
- 字段使用 `public`
- 结构体字段使用 `camelCase`（`type`、`code`）

### Editor 类命名

| 类型 | 规则 | 示例 |
|------|------|------|
| Inspector 扩展 | `<被扩展类名>Inspe` | `UMConfigInspe`、`UMEventInspe` |
| EditorWindow | `<功能>Window` | `UMConfigWindow`、`UMUIWindow` |

## 变量

| 类型 | 规范 | 示例 |
|------|------|------|
| 成员变量 | `private` + `m_` 前缀 + `camelCase` | `m_eventDic`、`m_reloadTimer`、`m_localDic` |
| Inspector 字段 | `[SerializeField] private` + `m_` 前缀，禁止 `public` | `[SerializeField] private float m_reloadTime` |
| Inspector 字段（子类可见） | `[SerializeField] protected` + `m_` 前缀 | `[SerializeField] protected string m_localID` |
| 常量 | `private const` + `PascalCase` | `CanvasName`、`EventSystemName` |
| 静态只读 | `private static readonly` + `PascalCase` | `SnapshotSettings` |
| 静态变量 | `PascalCase` | `TableDic`、`CachePanels`、`UILayers` |
| 局部变量 | `camelCase` | `bulletPrefab`、`shootPoint`、`fileFullPath` |

### Inspector 字段修饰

```csharp
[Header("分组名")] [Tooltip("说明文字")] [SerializeField]
private 类型 m_fieldName;
```

示例：
```csharp
[Header("开火参数")] [Tooltip("装弹时间（秒）—— 两次开火之间的最小间隔")] [SerializeField]
private float m_reloadTime = 1f;
```

## 代码组织

### 分区注释分隔符

```csharp
// ==================== 可序列化字段（Inspector 可编辑） ====================

// ==================== 私有字段（运行时状态） ====================

// ==================== 生命周期 ====================

// ==================== 逻辑 ====================

// ==================== 公开接口 ====================
```

功能密集的区域可使用分隔线注释：

```csharp
// ── 对外 API ──────────────────────────────────────────

// ── 生命周期 ──────────────────────────────────────────

// ── 辅助 ─────────────────────────────────────────────
```

### 成员排序

1. `[SerializeField] private` / `protected` 字段（带 `[Header]`、`[Tooltip]`）
2. `private` 字段（运行时状态）
3. `private const` / `private static readonly` 字段
4. `static` 字段
5. 属性（`public` 属性 / `protected` 属性）
6. 生命周期方法（`Awake`、`OnEnable`、`OnDisable`、`OnDestroy`、`Update` 等）
7. 逻辑方法（`private`）
8. 公开接口（`public` 方法 + Get/Set 访问器）

### #region 分组

对功能密集的类使用 `#region` / `#endregion` 分组（参考 `UMRes` 的 `Load`、`Instantiate`、`Unload`）。

## 框架核心模式

### UMMonoSingleton\<T\> — 模块单例基类

所有框架模块继承 `UMMonoSingleton<T>`，禁止外部 `new` / `AddComponent` 创建，仅通过 `Create()` 创建。

```csharp
public class UMRes : UMMonoSingleton<UMRes>
{
    protected override void OnInit()
    {
        // 初始化逻辑
    }

    // 静态 API 对外暴露
    public static T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }
}
```

关键规则：
- `Create()` / `Create(GameObject parent)` 标记 `internal`，由 `UMLauncher` 调用
- `Instance` 标记 `protected static`，子类内部使用
- 对外暴露 `public static` 方法/属性，内部通过 `Instance` 访问实例数据
- `IsCreated` 静态属性用于外部安全检查
- `OnInit()` 为抽象方法，子类必须实现
- `OnDestroy()` 可选 override，用于清理事件订阅等

### UMLauncher — 框架入口

```csharp
public static class UMLauncher
{
    public static bool IsWorked { get; private set; }

    public static void Work()
    {
        if (IsWorked) return;
        IsWorked = true;

        m_root = new GameObject("UMini");
        GameObject.DontDestroyOnLoad(m_root);

        // 创建框架模块
        UMConfig.Create(m_root);
        UMEvent.Create(m_root);
        UMPersist.Create(m_root);
        UMRes.Create(m_root);
        UMScene.Create(m_root);
        UMUI.Create(m_root);
    }
}
```

新增模块时在 `Work()` 中追加 `UMXxx.Create(m_root)`。

### 静态 API 封装模式

模块对外仅暴露 `public static` 方法，内部通过 `Instance` 访问实例字段：

```csharp
// 实例字段
private Dictionary<string, List<UMEventListener>> m_eventDic;

// 静态方法对外暴露
public static void AddEvent(string eventTag)
{
    if (Instance.m_eventDic.ContainsKey(eventTag)) return;
    Instance.m_eventDic.Add(eventTag, new List<UMEventListener>());
}
```

## Debug 日志

- 警告/错误使用 `$"[{ClassName}] message"` 前缀格式
- `Debug.LogError` 用于资源加载失败等严重问题
- `Debug.LogWarning` 用于可恢复的异常情况

```csharp
Debug.LogError($"[UMUI] 无法加载 Prefab: {cfg.PrefabPath}");
Debug.LogWarning($"[LocalCfg] SwitchByType 失败：未找到语言 '{type}'。");
```

## 注释规范

- 公开接口：`/// <summary>` XML 文档注释
- 复杂逻辑：行内注释，解释 **why** 而非 **what**
- 类头：多行 `/// <summary>` 描述职责和流程（编号列出关键步骤）

## Assembly Definition

- Runtime 程序集：`UMiniFramework.Runtime`（`UMiniFramework.Runtime.asmdef`）
- 引用 Newtonsoft.Json（通过 GUID）
- Editor 程序集隐式引用 Runtime

## 参考文件

- 类成员完整编码示例：[references/GunFireController.cs](references/GunFireController.cs)
