﻿// UMiniFramework config automatically generated, please do not modify it
using UMiniFramework.Runtime.UMEntrance;
using UMiniFramework.Runtime.Utils;
using UMiniFramework.Runtime.Modules.ConfigModule;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GameAudioTable : UMConfigTable
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    public const string ConfigAssetPath = "Game/Resources/Config/gameAudio";
    public const string ConfigLoadPath = "Config/gameAudio";

    /// <summary>
    /// 包含在配置表中的数据
    /// </summary>
    public List<GameAudioData> TableData { get; private set; }

    private Dictionary<string, GameAudioData> m_dataDicById;

    /// <summary>
    /// 通过 Id 属性查询数据
    /// </summary>
    public GameAudioData GetDataById(string id)
    {
        if (m_dataDicById.ContainsKey(id))
            return m_dataDicById[id];
        else
            UMUtilDebug.Warning($"GameAudioTable id does not exist {id}");
        return null;
    }

    public override IEnumerator Init()
    {
        m_dataDicById = new Dictionary<string, GameAudioData>();
        string jsonCofig = string.Empty;
        UMini.Asset.LoadAsync<TextAsset>(ConfigLoadPath, (configData) =>{
        if (configData != null)
        {
            jsonCofig = configData.Resource.text;
            TableData = JsonConvert.DeserializeObject<List<GameAudioData>>(jsonCofig);
            foreach (var data in TableData){
                m_dataDicById.Add(data.id, data);
            }
        UMUtilDebug.Log($"Init Config: {GetType().FullName} Succeed.");
        }
        else
        {
            UMUtilDebug.Warning($"config load failed. path: {ConfigLoadPath}");
        }});
        yield return new WaitUntil(() => { return TableData != null; });
    }
}
