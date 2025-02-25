// UMiniFramework config automatically generated, please do not modify it
using UMiniFramework.Runtime.Utils;
using UMiniFramework.Runtime.Modules.Config.Base;
using System.Collections.Generic;
using Newtonsoft.Json;

public class BulletTable : UMConfigTable
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    private const string ConfigAssetPath = "Assets/Game/Resources/ConfigData/bullet";
    public override string AssetPath { get { return ConfigAssetPath; } }
    private const string ConfigLoadPath = "ConfigData/bullet";
    public override string LoadPath { get { return ConfigLoadPath; } }

    /// <summary>
    /// 包含在配置表中的数据
    /// </summary>
    public List<BulletData> TableData { get; private set; }

    private Dictionary<string, BulletData> m_dataDicById;

    /// <summary>
    /// 通过 Id 属性查询数据
    /// </summary>
    public BulletData GetDataById(string id)
    {
        if (m_dataDicById.ContainsKey(id))
            return m_dataDicById[id];
        else
            UMUtilDebug.Warning($"BulletTable id does not exist {id}");
        return null;
    }

    protected override void Init(string tableContent)
    {
        m_dataDicById = new Dictionary<string, BulletData>();
        TableData = JsonConvert.DeserializeObject<List<BulletData>>(tableContent);
        for (var i = 0; i < TableData.Count; i++)
        {
            m_dataDicById.Add(TableData[i].id, TableData[i]);
        }
    }
}
