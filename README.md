# UMiniFramework Manual
- For Unity Mini Framework
---
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
- 使用方法待完善
# UI模块
- 使用方法待完善
# 其他
## UMiniConfig
- OnLaunchFinished
    - 设置框架启动完成的回调
- AssetLoader
    - 设置资源加载器
- ConfigTableList
    - 设置可以使用的配置表
- DataConverter
    - 设置持久化数据转换器
- DataPers
    - 设置持久化数据处理器
- IsPersiDataToConsole
    - 设置是否在控制台打印持久化数据的信息