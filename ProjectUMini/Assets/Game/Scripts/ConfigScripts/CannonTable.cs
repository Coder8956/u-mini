// UMiniFramework config automatically generated, please do not modify it
using UMiniFramework.Runtime.Utils;
using UMiniFramework.Runtime.Modules.Config.Base;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CannonTable : UMConfigTable
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    private const string ConfigAssetPath = "Assets/Game/Resources/ConfigData/cannon";
    public override string AssetPath { get { return ConfigAssetPath; } }
    private const string ConfigLoadPath = "ConfigData/cannon";
    public override string LoadPath { get { return ConfigLoadPath; } }

    /// <summary>
    /// 包含在配置表中的数据
    /// </summary>
    public List<CannonData> TableData { get; private set; }

    private Dictionary<string, CannonData> m_dataDicById;

    /// <summary>
    /// 通过 Id 属性查询数据
    /// </summary>
    public CannonData GetDataById(string id)
    {
        if (m_dataDicById.ContainsKey(id))
            return m_dataDicById[id];
        else
            UMUtilDebug.Warning($"CannonTable id does not exist {id}");
        return null;
    }

    protected override void Init(string tableContent)
    {
        m_dataDicById = new Dictionary<string, CannonData>();
        TableData = JsonConvert.DeserializeObject<List<CannonData>>(tableContent);
        for (var i = 0; i < TableData.Count; i++)
        {
            m_dataDicById.Add(TableData[i].id, TableData[i]);
        }
    }
}
