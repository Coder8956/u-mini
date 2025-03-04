# UMAudioClipInfo
- 用于记录音频信息

## 构造函数
```
UMAudioClipInfo(string id, string path, bool isPreLoad = false, UMResLoadType pathType = UMResLoadType.Resources)
```
- `id`设置音频id
- `path`音频路径
- `isPreLoad`是否预加载
- `pathType`路径类型
    - 目前只支持`UMResLoadType.Resources`