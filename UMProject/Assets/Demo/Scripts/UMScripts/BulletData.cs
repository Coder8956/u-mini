// UMiniFramework config automatically generated, please do not modify it
using Newtonsoft.Json;

public class BulletData
{
    /// <summary>
    /// 炮弹id
    /// </summary>
    [JsonProperty] public readonly string id;

    /// <summary>
    /// 攻击力
    /// </summary>
    [JsonProperty] public readonly int damage;

    /// <summary>
    /// 质量
    /// </summary>
    [JsonProperty] public readonly float mass;

    /// <summary>
    /// 初速度
    /// </summary>
    [JsonProperty] public readonly float initSpeed;

    /// <summary>
    /// 移动速度
    /// </summary>
    [JsonProperty] public readonly float moveSpeed;

    /// <summary>
    /// 炮弹预制体路径
    /// </summary>
    [JsonProperty] public readonly string prefabPath;

}
