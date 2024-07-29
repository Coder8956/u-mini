// UMiniFramework config automatically generated, please do not modify it
using UMiniFramework.Runtime.UMEntrance;
using UMiniFramework.Runtime.Utils;
using UMiniFramework.Runtime.Modules.ConfigModule;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class MonsterTable : UMConfigTable
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    public const string ConfigAssetPath = "Game/Resources/Config/monster";
    public const string ConfigLoadPath = "Config/monster";

    /// <summary>
    /// 包含在配置表中的数据
    /// </summary>
    public List<MonsterData> TableData { get; private set; }

    private Dictionary<string, MonsterData> m_dataDicById;

    /// <summary>
    /// 通过 Id 属性查询数据
    /// </summary>
    public MonsterData GetDataById(string id)
    {
        if (m_dataDicById.ContainsKey(id))
            return m_dataDicById[id];
        else
            UMUtilDebug.Warning($"MonsterTable id does not exist {id}");
        return null;
    }

    public override IEnumerator Init()
    {
        m_dataDicById = new Dictionary<string, MonsterData>();
        string jsonCofig = string.Empty;
        UMini.Asset.LoadAsync<TextAsset>(ConfigLoadPath, (configData) =>{
        if (configData != null)
        {
            jsonCofig = configData.Resource.text;
            TableData = JsonConvert.DeserializeObject<List<MonsterData>>(jsonCofig);
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
