using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor
{
    internal static class UMConfigScriptGenerator
    {
        private const string SCRIPT_TIP = "// UMiniFramework config automatically generated, please do not modify it";

        public static void CreateConfigScript(
            List<ConfigFieldInfo> fieldInfos,
            string excelPath,
            string scriptFolder,
            string dataFolder,
            List<string> tableClassList)
        {
            if (fieldInfos == null || fieldInfos.Count < 1) return;

            string excelName = Path.GetFileNameWithoutExtension(excelPath);
            string dataClassName = $"{UMConfigUtility.CapitalizeFirstWord(excelName)}Data";

            ConfigFieldInfo idConfigField = fieldInfos.Find(ele => ele.Field.ToLower() == "id");
            if (idConfigField == null)
            {
                EditorUtility.DisplayDialog("Tip",
                    $"Failed to generate the configuration table script. There is no id field in the configuration table.\n{excelPath}",
                    "OK");
                return;
            }

            GenerateDataScript(fieldInfos, dataClassName, scriptFolder);
            GenerateTableScript(fieldInfos, excelName, dataClassName, scriptFolder, dataFolder, tableClassList);
        }

        private static void GenerateDataScript(
            List<ConfigFieldInfo> fieldInfos,
            string dataClassName,
            string scriptFolder)
        {
            var sb = new StringBuilder();
            sb.AppendLine(SCRIPT_TIP);
            sb.AppendLine("using Newtonsoft.Json;");
            sb.AppendLine();
            sb.AppendLine($"public class {dataClassName}");
            sb.AppendLine("{");

            foreach (var cfi in fieldInfos)
            {
                if (!string.IsNullOrEmpty(cfi.Comments))
                {
                    sb.AppendLine("    /// <summary>");
                    sb.AppendLine($"    /// {cfi.Comments}");
                    sb.AppendLine("    /// </summary>");
                }

                sb.AppendLine($"    [JsonProperty] public readonly {cfi.Type.ToLower()} {cfi.Field};");
                sb.AppendLine();
            }

            sb.AppendLine("}");

            string savePath = $"{scriptFolder}/{dataClassName}.cs";
            if (File.Exists(savePath))
                File.Delete(savePath);

            File.WriteAllText(savePath, sb.ToString(), Encoding.UTF8);
        }

        private static void GenerateTableScript(
            List<ConfigFieldInfo> fieldInfos,
            string excelName,
            string dataClassName,
            string scriptFolder,
            string dataFolder,
            List<string> tableClassList)
        {
            string tableClassName = $"{UMConfigUtility.CapitalizeFirstWord(excelName)}Table";
            tableClassList.Add(tableClassName);

            string configPath = dataFolder.Replace($"{Application.dataPath}/", "");
            string configAssetPath = $"Assets/{configPath}/{excelName}";
            string[] splitStrs = configAssetPath.Split(new[] {"Resources/"}, System.StringSplitOptions.None);
            string configLoadPath = splitStrs[splitStrs.Length - 1];

            var sb = new StringBuilder();
            sb.AppendLine(SCRIPT_TIP);
            sb.AppendLine("using UMiniFramework.Runtime;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using Newtonsoft.Json;");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine();
            sb.AppendLine($"public class {tableClassName} : UMBaseConfigTable");
            sb.AppendLine("{");

            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 配置文件路径");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    private const string ConfigAssetPath = \"{configAssetPath}\";");
            sb.AppendLine("    public override string AssetPath { get { return ConfigAssetPath; } }");
            sb.AppendLine($"    private const string ConfigLoadPath = \"{configLoadPath}\";");
            sb.AppendLine("    public override string LoadPath { get { return ConfigLoadPath; } }");
            sb.AppendLine();

            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 包含在配置表中的数据");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public List<{dataClassName}> TableData {{ get; private set; }}");
            sb.AppendLine();

            // Find the id field (not necessarily the first column)
            ConfigFieldInfo idField = fieldInfos.Find(ele => ele.Field.ToLower() == "id");
            string idType = idField != null ? idField.Type.ToLower() : string.Empty;

            if (idField != null)
            {
                if (idType != "string")
                    return;

                string idDicTypeString = $"Dictionary<{idType}, {dataClassName}>";

                sb.AppendLine($"    private {idDicTypeString} m_dataDicById;");
                sb.AppendLine();
                sb.AppendLine("    /// <summary>");
                sb.AppendLine("    /// 通过 Id 属性查询数据");
                sb.AppendLine("    /// </summary>");
                sb.AppendLine($"    public {dataClassName} GetDataById({idType} id)");
                sb.AppendLine("    {");
                sb.AppendLine("        if (m_dataDicById.ContainsKey(id))");
                sb.AppendLine("            return m_dataDicById[id];");
                sb.AppendLine("        else");
                sb.AppendLine($"            Debug.LogWarning($\"{tableClassName} id does not exist {{id}}\");");
                sb.AppendLine("        return null;");
                sb.AppendLine("    }");
            }

            sb.AppendLine();
            sb.AppendLine("    protected override void OnInit(string tableContent)");
            sb.AppendLine("    {");
            sb.AppendLine($"        m_dataDicById = new Dictionary<{idType}, {dataClassName}>();");
            sb.AppendLine($"        TableData = JsonConvert.DeserializeObject<List<{dataClassName}>>(tableContent);");
            sb.AppendLine("        for (var i = 0; i < TableData.Count; i++)");
            sb.AppendLine("        {");
            sb.AppendLine("            m_dataDicById.Add(TableData[i].id, TableData[i]);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            string savePath = $"{scriptFolder}/{tableClassName}.cs";
            File.WriteAllText(savePath, sb.ToString(), Encoding.UTF8);
        }
    }
}
