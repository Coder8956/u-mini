using UnityEngine;

/// <summary>
/// 受击接口
/// 可被子弹击中的对象实现此接口，子弹碰撞时调用 OnHit 方法。
/// </summary>
public interface IHittable
{
    /// <summary>
    /// 被击中时调用
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="hitPoint">击中点（世界坐标）</param>
    /// <param name="hitDirection">击中方向（子弹飞行方向）</param>
    void OnHit(int damage, Vector3 hitPoint, Vector3 hitDirection);
}
