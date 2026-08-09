// UMiniFramework config automatically generated, please do not modify it
using UMiniFramework.Runtime;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LanguageCfg : UMBaseConfigTable
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    private const string ConfigAssetPath = "Assets/Demo/Resources/UMData/lang/lang_types";
    public override string AssetPath { get { return ConfigAssetPath; } }
    private const string ConfigLoadPath = "UMData/lang/lang_types";
    public override string LoadPath { get { return ConfigLoadPath; } }

    /// <summary>
    /// 语言类型列表
    /// </summary>
    private List<string> m_langTypes;

    /// <summary>
    /// 语言内容字典，key 为语言名称，value 为该语言的 id→内容映射
    /// </summary>
    private Dictionary<string, Dictionary<string, string>> m_langContent;

    /// <summary>
    /// 通过索引获取语言配置
    /// </summary>
    public Dictionary<string, string> GetContent(int index)
    {
        if (index < 0 || index >= m_langTypes.Count)
        {
            Debug.LogWarning($"Language index out of range: {index}");
            return null;
        }
        return m_langContent[m_langTypes[index]];
    }

    /// <summary>
    /// 通过语言名称获取语言配置
    /// </summary>
    public Dictionary<string, string> GetContent(string langName)
    {
        if (m_langContent.TryGetValue(langName, out var content))
            return content;
        Debug.LogWarning($"Language not found: {langName}");
        return null;
    }

    /// <summary>
    /// 通过索引和 id 获取单条语言文本
    /// </summary>
    public string GetText(int langIndex, string id)
    {
        var content = GetContent(langIndex);
        if (content != null && content.TryGetValue(id, out var text))
            return text;
        return null;
    }

    /// <summary>
    /// 通过语言名称和 id 获取单条语言文本
    /// </summary>
    public string GetText(string langName, string id)
    {
        var content = GetContent(langName);
        if (content != null && content.TryGetValue(id, out var text))
            return text;
        return null;
    }

    /// <summary>
    /// 获取所有语言种类
    /// </summary>
    public List<string> GetAllLanguages()
    {
        return new List<string>(m_langTypes);
    }

    /// <summary>
    /// 语言数量
    /// </summary>
    public int LanguageCount { get { return m_langTypes.Count; } }

    /// <summary>
    /// 通过索引获取语言名称
    /// </summary>
    public string GetLanguageName(int index)
    {
        if (index < 0 || index >= m_langTypes.Count)
            return null;
        return m_langTypes[index];
    }

    /// <summary>
    /// 通过语言名称获取索引
    /// </summary>
    public int GetLanguageIndex(string langName)
    {
        return m_langTypes.IndexOf(langName);
    }

    protected override void OnInit(string tableContent)
    {
        // 3.4.1: 读取语言类型数组
        m_langTypes = JsonConvert.DeserializeObject<List<string>>(tableContent);

        // 3.4.2: 读取所有语言内容
        m_langContent = new Dictionary<string, Dictionary<string, string>>();
        string basePath = ConfigLoadPath.Substring(0, ConfigLoadPath.LastIndexOf('/'));
        for (int i = 0; i < m_langTypes.Count; i++)
        {
            var asset = Resources.Load<TextAsset>($"{basePath}/lang_{i}");
            if (asset != null)
            {
                m_langContent[m_langTypes[i]] =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(asset.text);
            }
            else
            {
                Debug.LogWarning($"Language file not found: {basePath}/lang_{i}");
            }
        }
    }
}
