# UMiniFramework Manual
- For Unity Mini Framework
---
## 启动框架
- UMini.Launch(UMiniConfig config);
- UMiniConfig
    - OnLaunchFinished
        - 设置框架启动完成的回调
    - LaunchProgress
        - 获取框架启动进度的回调
    - AssetLoader
        - 设置资源加载器
        - 框架内置加载器:UMResourcesLoader
    - IsPersiDataToConsole
        - 设置是否在控制台打印持久化数据的信息
        - bool值:true/false
    - IsLog
        - 是否在控制台输出UMini相关的log
        - bool值:true/false
    - ConfigTableList
        - 设置在运行时需要读取的的配置表
    - PersistentDataList
        - 设置需要持久化的数据
    - EventTypeList
        - 设置事件类型
```
示例代码:
UMini.UMiniConfig umConfig = new UMini.UMiniConfig();

// UMini 启动完成后的回调
umConfig.OnLaunchFinished = () => 
            {
                UMUtils.Debug.Log("UMini Launch Finished."); 
            };

// UMini 框架启动进度的回调
umConfig.LaunchProgress = (progressTag, progressVal) =>
            {
                UMUtils.Debug.Log($"UMini LaunchProgress: {progressTag}, prograssVal: {progressVal}.");
            };

// 使用框架内置的资源加载器
umConfig.AssetLoader = new UMResourcesLoader();

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

// 设置需要持久化的数据
umConfig.PersistentDataList = new List<UMPersistentData>()
            {
                new UserData(),
                new GameData(),
                new UIData(),
            };

// 设置需要派发的事件
umConfig.EventTypeList = new List<string>()
            {
                GameEventConst.Launch,
                GameEventConst.Login
            };

// 启动框架
UMini.Launch(umConfig);
```
## Asset模块
- UMini.Asset.LoadAsync`<T>`(string path, Action<UMLoadResult`<T>`> onCompleted)
    - 异步加载资源
        - 参数path: 资源路径
        - 参数onCompleted: 资源加载成功后的回调
## Audio模块
### 特效音乐
- UMini.Audio.Effect.Play(string audioPath, float volume = 1)
    - 播放特效音
    - 参数audioPath: 音频路径
    - 参数volume: 音量
- UMini.Audio.Effect.GetMute()
    - 获取特效音静音状态
- UMini.Audio.Effect.SetMute(bool val)
    - 设置特效音静音状态
### 背景音乐
- UMini.Audio.BGM.Play(string audioPath, float volume = 1, bool loop = true)
    - 播放背景音乐
    - 参数audioPath: 音频路径
    - 参数volume: 音量
    - 参数loop: 是否循环
- UMini.Audio.BGM.Stop()
    - 停止播放背景音乐
- UMini.Audio.BGM.GetMute()
    - 获取背景音乐静音状态
- UMini.Audio.BGM.SetMute(bool val)
    - 设置背景音乐静音状态
## Config模块
- UMini.Config.GetTable`<T>`()
    - 获取配置表数据
    - 需要获取的数据必须已经在框架启动时进行了注册
## PersistentData模块
- 编写需要持久化的类
    ```
    示例代码:
    public class UserData : UMPersistentData
    {
        public int Age;
        public string Name;
        public bool Sex;
    }
    代码解释:
    - 持久化数据类必须继承 UMPersistentData
    - 需要持久化的字段访问权限设置为 public
        - 或者参考 NewtonsoftJson 序列化
        - 数据序列化最终用的是 NewtonsoftJson 提供的 API
    ```
- UMini.PersiData.Query`<T>`()
    - 查询数据
- UMini.PersiData.Save`<T>`()
    - 保存数据
- UMini.PersiData.ResetDefault`<T>`()
    - 重置数据为默认值
- UMini.PersiData.SaveAllData()
    - 保存所有数据
## Scene模块
- UMini.Scene.Load(string scene)
    - 同步加载场景
        - 参数scene: 场景名字
- UMini.Scene.LoadSceneAsync(string scene)
    - 异步加载场景
        - 参数scene: 场景名字
## UI模块
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
- Panel基类
    - UMUIPanel
- Dialog基类
    - UMUIDialog
- UMini.Scene.Open`<T>`(Action`<T>` completed = null)
    - 打开UI
        - 参数T: 窗口类
        - 参数completed: 窗口打开的回调
- UMini.Scene.Close`<T>`()
    - 关闭UI
        - 参数T: 窗口类
## Event模块
- UMini.Event.AddListener(string eventType, IUMEventListener listener, UMListenType type);
    - 添加侦听器
        - 参数eventType: 事件类型
        - 参数listener: 侦听器对象
        - 参数type: 侦听类型(一次/多次)
- UMini.Event.Dispatch(string eventType, UMEventBody eventBody);
    - 派发事件
        - 参数eventType: 事件类型
        - 参数eventBody: 事件参数
- UMini.Event.RemoveListener(string eventType, IUMEventListener listener);
    - 移除侦听器
        - 参数eventType: 事件类型
        - 参数listener: 侦听器对象
- UMini.Event.RemoveAllListener();
    - 移除所有侦听器
- UMini.Event.RemoveAllListenerByEvnetType(string eventType);
    - 移除指定事件的所有侦听器
        - 参数eventType: 事件类型
- 侦听对象需要继承 IUMEventListener
    ```
    示例代码:
    public class DemoUMListener : IUMEventListener
    {
        public void UMOnReceiveEvent(UMEvent content)
        {
            // 处理侦听到的事件
            EBDebug body = content.Body as EBDebug;
        }
    }
    代码解释:
    - content.Body 的数据类型是 UMEventBody
        - UMEventBody 是消息体基类, 可以继承 UMEventBody 后实现自己的消息体, 本例中的自定义实现为 EBDebug
    ```