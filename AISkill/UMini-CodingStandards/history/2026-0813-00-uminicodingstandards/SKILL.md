# Unity UMini框架 C# 编码规范

## 根目录
- UMiniFramework

## Runtime 目录(运行时目录)

### 命名空间
- 固定使用 UMiniFramework.Runtime

### 类
- 使用 `PascalCase` 命名规则,添加前缀UM,如:资源模块`UMRes`

### 接口
- 命名：`IUM` + 名词/形容词（如 `IUMDamageable`、`IUMInteractable`）
- 优先使用"能力"命名——接口名描述对象"能做什么"
- 示例：
  ```csharp
  public interface IUMDamageable
  {
      void TakeDamage(int damage);
  }
  ```
  语义：`IUMDamageable` = "这个对象是可以受到伤害的"

### 基类
- 命名：UM + 功能名称 + `Base`
- 示例：
  ```
  UMWeaponBase
  UMUIBase
  UMManagerBase
  UMSystemBase
  ```

### 变量

#### 成员变量
- 必须私有（`private`）
- 命名规范：`m_varName`（`m_` 前缀 + camelCase）
- Inspector 可编辑字段使用 `[SerializeField] private`，不使用 `public`

#### 常量
- 使用 `PascalCase` 命名规则
- 使用 `const` 或 `static readonly`

#### 静态变量
- 使用 `PascalCase` 命名规则

#### 注释
- 注释以便于理解为主，解释 why 而非 what
- 公开接口使用 `/// <summary>` XML 文档注释
- 复杂逻辑添加行内注释

#### 代码组织
- 使用注释分隔符分区（与示例一致）：
  - `// ==================== 可序列化字段（Inspector 可编辑） ====================`
  - `// ==================== 私有字段（运行时状态） ====================`
  - `// ==================== 生命周期 ====================`
  - `// ==================== 逻辑 ====================`
  - `// ==================== 公开接口 ====================`
- 公开方法提供对应的 Get/Set 访问器

#### 类成员编码示例
- 类成员完整示例见 [GunFireController.cs](references/GunFireController.cs)

## Editor 目录(编辑器扩展目录)

### 命名空间
- 固定使用 UMiniFramework.Editor

### 类
- 类名使用 `PascalCase` 命名规则
- 添加后缀
  - Inspector 扩展类命名规则：`<被扩展类名>Inspe`，如 `GunInspector`
  - EditorWindow 扩展类命名规则：`<功能>Window`，如 `GunWindow`

### 其他规范
- 无特殊说明统一参照Runtime目录规范