// UMiniFramework config automatically generated, please do not modify it

using System.Collections.Generic;
using Newtonsoft.Json;
using UMiniFramework.Scripts;
using UMiniFramework.Scripts.Utils;
using UnityEngine;


public class BulletTable
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    public const string ConfigAssetPath = "Game/Resources/Config/bullet";
    public const string ConfigLoadPath = "Config/bullet";

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
            UMUtils.Debug.Warning($"BulletTable id does not exist {id}");
        return null;
    }

    public void Init()
    {
        m_dataDicById = new Dictionary<string, BulletData>();
        string jsonCofig = string.Empty;
        UMini.Asset.LoadAsync<TextAsset>(ConfigLoadPath, (configData) =>{
        if (configData != null)
        {
            jsonCofig = configData.Resource.text;
            TableData = JsonConvert.DeserializeObject<List<BulletData>>(jsonCofig);
            foreach (var data in TableData){
                m_dataDicById.Add(data.id, data);
            }
        }
        else
        {
            UMUtils.Debug.Warning($"config load failed. path: {ConfigLoadPath}");
        }
        UMUtils.Debug.Log($"Init Config: {GetType().FullName}");});
    }
}
