// UMiniFramework config automatically generated, please do not modify it
using UMiniFramework.Runtime;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class MonsterTable : UMConfigTableBase
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    private const string ConfigAssetPath = "Assets/Demo/Resources/UMData/monster";
    public override string AssetPath { get { return ConfigAssetPath; } }
    private const string ConfigLoadPath = "UMData/monster";
    public override string LoadPath { get { return ConfigLoadPath; } }

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
            Debug.LogWarning($"MonsterTable id does not exist {id}");
        return null;
    }

    protected override void OnInit(string tableContent)
    {
        m_dataDicById = new Dictionary<string, MonsterData>();
        TableData = JsonConvert.DeserializeObject<List<MonsterData>>(tableContent);
        for (var i = 0; i < TableData.Count; i++)
        {
            m_dataDicById.Add(TableData[i].id, TableData[i]);
        }
    }
}
