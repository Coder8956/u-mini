# UMGOP
- UM游戏物体对象池(UM GameObject Pool)

## 属性

### OnGet
- 委托字段.在获取游戏物体时调用

### OnBack
- 委托字段.在返回游戏物体时调用

## 功能

### Get
- 获取游戏物体
```
GameObject Get()
```
- `GameObject`返回值是一个游戏物体

### Back
- 放回游戏物体
```
void Back(GameObject backGO)
```
- `backGO`要放回的游戏物体