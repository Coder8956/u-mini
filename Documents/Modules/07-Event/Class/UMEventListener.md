# UMEventListener
- 事件侦听器

## 构造函数
```
UMEventListener(string eventTag, UMListenType listenType, UnityAction<UMBaseEventContent> eventHandler)
```
- `eventTag`事件标记
- `listenType`侦听类型
    - UMListenType.Once 侦听一次
    - UMListenType.Persistent 持续侦听
- `eventHandler`事件处理器

## 属性

### EventTag
- 事件标记

### ListenType
- 侦听类型