# UMini 设计文档
> 通过 框架管理器模块 控制其他模块, 每个功能模块可以独立运行, 各个模块之间没有任何依赖.

## 框架启动示例
```C#
private void Start()
{
    // 启动 UMini 框架
    UMGR.Launch();
    // 注册模块(不传参数)
    UMGR.Register<UMUI>();
    UMGR.Register<UMAudio>();
    UMGR.Register<UMScene>();
    UMGR.Register<UMDataPer>();
    UMGR.Register<UMConfig>();
    UMGR.Register<UMRes>();
    UMGR.Register<UMEvent>();
    UMGR.Register<UMGOPools>();
    // 初始化注册的模块
    UMGR.InitModules(UMGRMIPHandler);
}

private void UMGRMIPHandler(InitProgressInfo info)
{
    UMBaseModule module = info.InitModule;
    float initProgress = info.InitProgress;
    if (!info.InitState)
    {
        UMModuleType moduleType = module.ModuleType;
        string moduleTypeStr = moduleType.ToString();
        Debug.Log($"Init modules progress: {initProgress}. module: {moduleTypeStr}");
    }
    else
    {
        // 处理初始化完成的状态
        Debug.Log($"Init modules progress: {initProgress}. modules init finished.");
        GameLaunchFunc();
    }
}

private void GameLaunchFunc()
{
    // 处理游戏启动流程
}
```

## 命名规范

### 功能模块的命名
- `UM`+`功能`
- 例如: 框架管理器模块 `UMManager`

## 管理器模块
- [文档-管理器模块](./Modules/00-Manager/00-管理器模块.md)

## UI模块
- [文档-UI模块](./Modules/01-UI/00-UI模块.md)

## 音频模块
- [文档-音频模块](./Modules/02-Audio/00-音频模块.md)

## 场景模块
- [文档-场景模块](./Modules/03-Scene/00-场景模块.md)

## 数据持久化模块
- [文档-数据持久化模块](./Modules/04-DataPersistence/00-数据持久化模块.md)

## 配置模块
- [文档-配置模块](./Modules/05-Config/00-配置模块.md)

## 资源模块
- [文档-资源模块](./Modules/06-Resource/00-资源模块.md)

## 事件模块
- [文档-事件模块](./Modules/07-Event/00-事件模块.md)

## 对象池模块
- [文档-对象池模块](./Modules/08-GOPools/00-对象池模块.md)

## 框架初始化
```C#

```