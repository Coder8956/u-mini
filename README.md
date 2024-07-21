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
- UMini.Asset.LoadAsync<T>(string path, Action<LoadResult<T>> onCompleted)
    - 异步加载资源
        - 参数path: 资源路径
        - 参数onCompleted: 资源加载成功后的回调
# Audio模块
- 使用方法待完善
# Config模块
- 使用方法待完善
# PersistentData模块
- 使用方法待完善
# Scene模块
- UMini.Scene.Load(string scene)
    - 同步加载场景
        - 参数scene: 场景名字
- UMini.Scene.LoadSceneAsync(string scene)
    - 异步加载场景
        - 参数scene: 场景名字
# UI模块
- 编写UI窗口并挂载到预制体上
    ```
    示例代码:
    [UMUIPanelInfo("Prefabs/UI/PanelLogin", UMUILayer.Middle)]
    public class PanelLogin : UMUIPanel
    {
        public override void OnLoaded()
        {
            // 整个生命周期执行一次
        }

        public override void OnOpen()
        {
            // 每次打开执行一次
        }

        public override void OnClose()
        {
            // 每次关闭执行一次
        }
    }
    代码解释:
    - [UMUIPanelInfo("填写预制体路径", 设置UI在哪一层打开)]
    - 窗口类必须继承 UMUIPanel
    ```
- UMini.Scene.Open<T>(Action<T> completed = null)
    - 打开UI
        - 参数T: 窗口类
        - 参数completed: 窗口打开的回调
- UMini.Scene.Close<T>()
    - 关闭UI
        - 参数T: 窗口类