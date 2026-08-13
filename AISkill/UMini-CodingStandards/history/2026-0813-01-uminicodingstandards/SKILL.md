---
name: uminicodingstandards
description: UMiniFramework C# 编码规范。在编写或修改 UMiniFramework 相关 C# 脚本时自动激活，确保 UM 前缀命名、UMMonoSingleton 单例模式、模块静态 API 封装、UI 面板 UMUIPanelBase + UMUIPanelCfg 特性驱动、配置表/事件/持久化基类继承、Inspector 字段 [SerializeField] private、成员变量 m_ 前缀、分区注释分隔符等规范。触发于"写脚本"、"创建脚本"、"代码规范"、"新建模块"、"新建 UI 面板"、"UMini"、"UM"等。
---

# UMiniFramework 编码规范
## 框架根目录
- Assets/UMiniFramework
## 命名空间

| 目录 | 命名空间 |
|------|----------|
| Runtime | `UMiniFramework.Runtime` |
| Editor | `UMiniFramework.Editor` |

## 命名规范

### 类
- `PascalCase`，统一添加 `UM` 前缀
- 模块管理器：`UMC` + 功能名（`UMCRes`、`UMCUI`、`UMCEvent`、`UMCConfig`、`UMCPersist`、`UMCScene`）
    - UMC(UM Module Core)
- 普通业务脚本：`PascalCase`，加 `UM` 前缀（如 `UMGunFireController`）

### 接口
- `IUM` + 能力名词/形容词（`IUMLangTable`、`IUMDamageable`、`IUMInteractable`）
- 优先"能做什么"语义

### 基类
- `UM`前缀 + 功能名 + `Base`后缀（`UMUIPanelBase`、`UMConfigTableBase`、`UMEventContentBase`、`UMPersistDataBase`）

### 枚举
- `UM` 前缀 + `PascalCase`（`UMListenType`）
- 枚举值 `PascalCase`，带 `/// <summary>` XML 注释

### 结构体
- `UM`前缀 + `PascalCase`，字段使用 `public`（`UMLangOption`）

### 变量

| 类型 | 规范 | 示例 |
|------|------|------|
| 成员变量 | `private` + `m_` 前缀 + `camelCase` | `m_eventDic`、`m_reloadTimer` |
| Inspector 字段 | `[SerializeField] private` + `m_` 前缀，禁止 `public` | `[SerializeField] private float m_reloadTime` |
| 常量 | `const` / `static readonly` + `PascalCase` | `CanvasName` |
| 静态变量 | `PascalCase` | `TableDic`、`IsWorked` |
| 局部变量 | `camelCase` | `bulletPrefab`、`shootPoint` |

### Editor 类命名

| 类型 | 规则 | 示例 |
|------|------|------|
| Inspector 扩展 | `<被扩展类名>Inspe` | `UMConfigInspe`、`UMEventInspe` |
| EditorWindow | `<功能>Window` | `UMConfigWindow`、`UMUIWindow` |

## 代码组织

### 分区注释分隔符

```csharp
// ==================== 可序列化字段（Inspector 可编辑） ====================

// ==================== 私有字段（运行时状态） ====================

// ==================== 生命周期 ====================

// ==================== 逻辑 ====================

// ==================== 公开接口 ====================
```

### 成员排序

1. `[SerializeField] private` 字段（带 `[Header]`、`[Tooltip]`）
2. `private` 字段（运行时状态）
3. 静态字段 / 常量
4. 属性
5. 生命周期方法（`Awake`、`OnEnable`、`OnDisable`、`Update` 等）
6. 逻辑方法（`private`）
7. 公开接口（`public` 方法 + Get/Set 访问器）

### #region 分组

对功能密集的类使用 `#region` / `#endregion` 分组（参考 `UMRes` 的 `Load`、`Instantiate`、`Unload`）。

## 框架核心模式

### UMMonoSingleton — 模块单例基类

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
- `Instance` 标记 `protected`，子类内部使用
- 对外暴露 `public static` 方法/属性，内部通过 `Instance` 访问实例数据
- `OnInit()` 为抽象方法，子类必须实现

### UMLauncher — 框架入口

```csharp
public static class UMLauncher
{
    public static bool IsWorked { get; private set; }

    public static void Work()
    {
        if (IsWorked) return;
        IsWorked = true;

        // 创建根节点
        // 依次创建各模块单例，挂载到根节点下
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

## 注释规范

- 公开接口：`/// <summary>` XML 文档注释
- 复杂逻辑：行内注释，解释 **why** 而非 **what**
- 类头：多行 `/// <summary>` 描述职责和流程

## Debug 日志

- 警告/错误使用 `$"[{ClassName}] message"` 前缀格式
- `Debug.LogError` 用于资源加载失败等严重问题
- `Debug.LogWarning` 用于可恢复的异常情况

## 参考文件

- 类成员完整编码示例：[references/GunFireController.cs](references/GunFireController.cs)
