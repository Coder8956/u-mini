# UMiniFramework Manual
- For Unity Mini Framework
---
# 启动框架
- UMini.Launch(UMiniConfig config);
- UMiniConfig
    - OnLaunchFinished
        - 设置框架启动完成的回调
    - AssetLoader
        - 设置资源加载器
        - 框架内置加载器:UMResourcesLoader
    - ConfigTableList
        - 设置在运行时需要读取的的配置表
    - DataConverter
        - 设置持久化数据转换器
        - 框架内置转换器:UMDefaultDataConverter
    - DataPers
        - 设置持久化数据处理器
        - 框架内置处理器:UMDefaultDataPers
    - IsPersiDataToConsole
        - 设置是否在控制台打印持久化数据的信息
        - bool值:true/false
    - IsLog
        - 是否在控制台输出UMini相关的log
        - bool值:true/false
```
示例代码:
UMini.UMiniConfig umConfig = new UMini.UMiniConfig();

umConfig.OnLaunchFinished = () => {
    // UMini 启动完成后的回调
    UMUtils.Debug.Log("UMini Launch Finished."); };

// 使用框架内置的资源加载器
umConfig.AssetLoader = new UMResourcesLoader();

// 使用框架内置的数据转换器
umConfig.DataConverter = new UMDefaultDataConverter();

// 使用框架内置的数据处理器
umConfig.DataPers = new UMDefaultDataPers();

// 在控制台打印数据log
umConfig.IsPersiDataToConsole = true;

// 输出UMini框架相关的log
umConfig.IsLog = true;

// 设置在运行时要使用的配置表
umConfig.ConfigTableList = new List<UMConfigTable>()
            {
                new LevelTable(),
                new GameAudioTable(),
                new CannonTable(),
                new BulletTable(),
                new MonsterTable(),
            };

// 启动框架
UMini.Launch(umConfig);
```
# Asset模块
- LoadAsync<T>(string path, Action<LoadResult<T>> onCompleted)
    - 异步加载资源 path 资源路径; onCompleted 资源加载成功后的回调
# Audio模块
- 使用方法待完善
# Config模块
- 使用方法待完善
# PersistentData模块
- 使用方法待完善
# Scene模块
- Load(string scene)
    - 同步加载场景
- LoadSceneAsync(string scene)
    - 异步加载场景
# UI模块
- 使用方法待完善