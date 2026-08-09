// UMiniFramework config automatically generated, please do not modify it
using UMiniFramework.Runtime;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LanguageCfg : UMBaseConfigTable, IUMLangTable
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

    private class LangTypeEntry
    {
        [JsonProperty("type")] public string type;
        [JsonProperty("code")] public string code;
        [JsonProperty("file")] public string file;
    }

    private class LangEntry
    {
        [JsonProperty("type")] public string type;
        [JsonProperty("code")] public string code;
        [JsonProperty("content")] public Dictionary<string, string> content;
    }

    /// <summary>
    /// 语言文件名列表
    /// </summary>
    private List<string> m_langFiles;

    /// <summary>
    /// 语言代码列表
    /// </summary>
    private List<string> m_langCodes;

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
    /// 获取所有语言选项（类型 + 代码）
    /// </summary>
    public List<LangOption> GetOptions()
    {
        var options = new List<LangOption>(m_langTypes.Count);
        for (int i = 0; i < m_langTypes.Count; i++)
        {
            options.Add(new LangOption(m_langTypes[i], m_langCodes[i]));
        }
        return options;
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

    /// <summary>
    /// 通过索引获取语言代码
    /// </summary>
    public string GetLanguageCode(int index)
    {
        if (index < 0 || index >= m_langCodes.Count)
            return null;
        return m_langCodes[index];
    }

    /// <summary>
    /// 通过索引获取语言对应的配置文件名
    /// </summary>
    public string GetLanguageFile(int index)
    {
        if (index < 0 || index >= m_langFiles.Count)
            return null;
        return m_langFiles[index];
    }

    protected override void OnInit(string tableContent)
    {
        var langEntries = JsonConvert.DeserializeObject<List<LangTypeEntry>>(tableContent);

        m_langTypes = new List<string>(langEntries.Count);
        m_langFiles = new List<string>(langEntries.Count);
        m_langCodes = new List<string>(langEntries.Count);
        m_langContent = new Dictionary<string, Dictionary<string, string>>();

        string basePath = ConfigLoadPath.Substring(0, ConfigLoadPath.LastIndexOf('/'));
        for (int i = 0; i < langEntries.Count; i++)
        {
            var entry = langEntries[i];
            m_langTypes.Add(entry.type);
            m_langFiles.Add(entry.file);

            string fileName = entry.file.EndsWith(".json")
                ? entry.file.Substring(0, entry.file.Length - 5)
                : entry.file;
            var asset = Resources.Load<TextAsset>($"{basePath}/{fileName}");
            if (asset != null)
            {
                var langData = JsonConvert.DeserializeObject<LangEntry>(asset.text);
                m_langCodes.Add(langData.code);
                m_langContent[entry.type] = langData.content;
            }
            else
            {
                Debug.LogWarning($"Language file not found: {basePath}/{entry.file}");
            }
        }
    }
}
