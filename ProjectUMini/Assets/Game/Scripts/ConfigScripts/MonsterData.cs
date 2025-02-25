// UMiniFramework config automatically generated, please do not modify it
using Newtonsoft.Json;

public class MonsterData
{
    /// <summary>
    /// 怪物id
    /// </summary>
    [JsonProperty] public readonly string id;

    /// <summary>
    /// 怪物预制体资源路径
    /// </summary>
    [JsonProperty] public readonly string monsterPath;

    /// <summary>
    /// 怪物类型(0小怪)(1精英)(2Boss)
    /// </summary>
    [JsonProperty] public readonly int type;

}
