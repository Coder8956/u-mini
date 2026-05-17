// UMiniFramework config automatically generated, please do not modify it

using System.Collections.Generic;
using Newtonsoft.Json;
using UMiniFramework.Runtime.Modules;
using UMiniFramework.Runtime.Utils;

public class BlockTable : UMConfigTable
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    private const string ConfigAssetPath = "Assets/Game/Resources/ConfigData/block";

    public override string AssetPath
    {
        get { return ConfigAssetPath; }
    }

    private const string ConfigLoadPath = "ConfigData/block";

    public override string LoadPath
    {
        get { return ConfigLoadPath; }
    }

    /// <summary>
    /// 包含在配置表中的数据
    /// </summary>
    public List<BlockData> TableData { get; private set; }

    private Dictionary<string, BlockData> m_dataDicById;

    /// <summary>
    /// 通过 Id 属性查询数据
    /// </summary>
    public BlockData GetDataById(string id)
    {
        if (m_dataDicById.ContainsKey(id))
            return m_dataDicById[id];
        else
            UMUtilDebug.Warning($"BlockTable id does not exist {id}");
        return null;
    }

    protected override void Init(string tableContent)
    {
        m_dataDicById = new Dictionary<string, BlockData>();
        TableData = JsonConvert.DeserializeObject<List<BlockData>>(tableContent);
        for (var i = 0; i < TableData.Count; i++)
        {
            m_dataDicById.Add(TableData[i].id, TableData[i]);
        }
    }
}