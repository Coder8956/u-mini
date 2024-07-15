// UMiniFramework config automatically generated, please do not modify it
using Newtonsoft.Json;

public class MonsterData
{
    /// <summary>
    /// 怪物id
    /// </summary>
    [JsonProperty] public readonly string id;

    /// <summary>
    /// 怪物名字
    /// </summary>
    [JsonProperty] public readonly string[] name;

    /// <summary>
    /// 武器开关
    /// </summary>
    [JsonProperty] public readonly bool[] weaponsSwitch;

    /// <summary>
    /// 初始化数量
    /// </summary>
    [JsonProperty] public readonly int[] initNum;

    /// <summary>
    /// 技能刷新时间
    /// </summary>
    [JsonProperty] public readonly float[] skillCD;

}
