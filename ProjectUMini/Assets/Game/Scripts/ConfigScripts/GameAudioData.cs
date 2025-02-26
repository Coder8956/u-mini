// UMiniFramework config automatically generated, please do not modify it
using Newtonsoft.Json;

public class GameAudioData
{
    /// <summary>
    /// 音效id
    /// </summary>
    [JsonProperty] public readonly string id;

    /// <summary>
    /// 路径
    /// </summary>
    [JsonProperty] public readonly string path;

    /// <summary>
    /// 音乐类型. 0-bgm; 1-音效
    /// </summary>
    [JsonProperty] public readonly int type;

    /// <summary>
    /// 是否循环
    /// </summary>
    [JsonProperty] public readonly bool loop;

}
