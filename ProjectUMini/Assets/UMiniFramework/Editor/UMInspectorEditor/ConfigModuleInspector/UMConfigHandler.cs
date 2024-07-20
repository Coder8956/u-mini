using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ExcelDataReader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UMiniFramework.Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMInspectorEditor.ConfigModuleInspector
{
    public class UMConfigHandler
    {
        private static string ExcelFolder;
        private static string ScriptFolder;
        private static string DataFolder;
        private const string ARR_SPLIT = ";";
        private static List<string> TableClassList = new List<string>();
        private static string ScriptTip = "// UMiniFramework config automatically generated, please do not modify it";

        public static void UpdateConfig(string excelFolder, string scriptFolder, string dataFolder)
        {
            ExcelFolder = excelFolder.Replace("\\", "/");
            ScriptFolder = scriptFolder.Replace("\\", "/");
            DataFolder = dataFolder.Replace("\\", "/");

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("[ExcelFolder]:");
            stringBuilder.AppendLine(ExcelFolder);
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("[ScriptFolder]:");
            stringBuilder.AppendLine(ScriptFolder);
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("[DataFolder]:");
            stringBuilder.AppendLine(DataFolder);

            bool isOK = EditorUtility.DisplayDialog("Update Config", stringBuilder.ToString(), "Ok", "Cancel");
            if (!isOK) return;

            if (!Directory.Exists(excelFolder))
            {
                EditorUtility.DisplayDialog("无效路径", scriptFolder, "确认");
                return;
            }

            if (!Directory.Exists(scriptFolder))
            {
                EditorUtility.DisplayDialog("无效路径", scriptFolder, "确认");
                return;
            }

            if (!Directory.Exists(dataFolder))
            {
                EditorUtility.DisplayDialog("无效路径", dataFolder, "确认");
                return;
            }

            // 清理 脚本 和 数据 文件夹
            Directory.Delete(scriptFolder, true);
            Directory.CreateDirectory(scriptFolder);

            Directory.Delete(dataFolder, true);
            Directory.CreateDirectory(dataFolder);

            UMUtils.Debug.Log($"开始生成配置");
            UMUtils.Debug.Log($"ExcelFolder:{ExcelFolder}");
            UMUtils.Debug.Log($"ScriptFolder:{ScriptFolder}");
            UMUtils.Debug.Log($"DataFolder:{DataFolder}");

            List<string> excels = new List<string>();

            GetAllExcelFiles(ExcelFolder, excels);

            foreach (var excel in excels)
            {
                UMUtils.Debug.Log($"config excel: {excel}");
            }

            UMUtils.Debug.Log($"config excel count: {excels.Count}");
            TableClassList.Clear();
            for (var i = 0; i < excels.Count; i++)
            {
                string currentExcel = excels[i];
                EditorUtility.DisplayProgressBar("UMConfigModule Create Config By Excel",
                    $"Current Excel: {currentExcel}", i / excels.Count);
                CreateConfigByExcel(currentExcel);
            }

            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }

        private static void CreateConfigByExcel(string excel)
        {
            if (excel.Contains("~$")) return;
            UMUtils.Debug.Log($"create config by excel : {excel}");
            using (var stream = File.Open(excel, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    if (result.Tables.Count > 0)
                    {
                        DataTable table = result.Tables[0];

                        // 注释行
                        DataRow commentsRow = table.Rows[0];
                        // 字段行
                        DataRow fieldRow = table.Rows[1];
                        // 字段类型行
                        DataRow typeRow = table.Rows[2];

                        int validColumnCount = 0;

                        for (var i = 0; i < table.Columns.Count; i++)
                        {
                            if (UMUtils.String.IsValid(fieldRow[i].ToString()))
                            {
                                ++validColumnCount;
                            }
                            else
                            {
                                break;
                            }
                        }

                        UMUtils.Debug.Log($"validColumnCount: {validColumnCount}");

                        List<ConfigFieldInfo> fieldInfos = new List<ConfigFieldInfo>();

                        for (int i = 0; i < validColumnCount; i++)
                        {
                            fieldInfos.Add(new ConfigFieldInfo(commentsRow[i].ToString(), fieldRow[i].ToString(),
                                typeRow[i].ToString(), i));
                        }

                        //  根据excel生成Json                        
                        CreateConfigJson(fieldInfos, table, excel);

                        // 创建配置脚本
                        CreateConfigScript(fieldInfos, excel);
                    }
                }
            }
        }

        private static void CreateConfigJson(List<ConfigFieldInfo> fieldInfos, DataTable table, string excelPath)
        {
            string excelName = Path.GetFileNameWithoutExtension(excelPath);
            UMUtils.Debug.Log($"Create config json excelName: {excelName}");
            if (fieldInfos == null || fieldInfos.Count < 1) return;
            string jsonConfigPath = $"{DataFolder}/{excelName}.json";

            JArray configJson = new JArray();

            // 从第三行开始读,第三行开始为数据行
            for (var i = 3; i < table.Rows.Count; i++)
            {
                JObject dataItem = new JObject();

                bool isVaildRow = true;
                for (int j = 0; j < fieldInfos.Count; j++)
                {
                    // 检查是否是有效行
                    if (!UMUtils.String.IsValid(table.Rows[i][0].ToString()))
                    {
                        isVaildRow = false;
                        break;
                    }
                    else
                    {
                        ConfigFieldInfo cfi = fieldInfos[j];
                        string currentKey = cfi.Field;
                        string currentValue = table.Rows[i][cfi.ColumnIndex].ToString();

                        switch (cfi.Type.ToLower())
                        {
                            case "string":
                                dataItem[currentKey] = currentValue;
                                break;
                            case "string[]":
                                JArray stringJArray = new JArray();
                                string[] strArr = currentValue.Split(ARR_SPLIT);
                                foreach (var s in strArr)
                                {
                                    if (UMUtils.String.IsValid(s))
                                    {
                                        stringJArray.Add(s);
                                    }
                                }

                                dataItem[currentKey] = stringJArray;
                                break;
                            case "float":
                                dataItem[currentKey] = float.Parse(currentValue);
                                break;
                            case "float[]":
                                JArray floatJArray = new JArray();
                                string[] strFloatArr = currentValue.Split(ARR_SPLIT);
                                foreach (var s in strFloatArr)
                                {
                                    if (UMUtils.String.IsValid(s))
                                    {
                                        floatJArray.Add(float.Parse(s));
                                    }
                                }

                                dataItem[currentKey] = floatJArray;
                                break;
                            case "int":
                                dataItem[currentKey] = int.Parse(currentValue);
                                break;
                            case "int[]":
                                JArray intJArray = new JArray();
                                string[] strIntArr = currentValue.Split(ARR_SPLIT);
                                foreach (var s in strIntArr)
                                {
                                    if (UMUtils.String.IsValid(s))
                                    {
                                        intJArray.Add(int.Parse(s));
                                    }
                                }

                                dataItem[currentKey] = intJArray;
                                break;
                            case "bool":
                                dataItem[currentKey] = bool.Parse(currentValue);
                                break;
                            case "bool[]":
                                JArray boolJArray = new JArray();
                                string[] strBoolArr = currentValue.Split(ARR_SPLIT);
                                foreach (var s in strBoolArr)
                                {
                                    if (UMUtils.String.IsValid(s))
                                    {
                                        boolJArray.Add(bool.Parse(s));
                                    }
                                }

                                dataItem[currentKey] = boolJArray;
                                break;
                            default:
                                UMUtils.Debug.Log(
                                    $"Invaild fieldType: {cfi.Type.ToLower()}, excel path: {excelPath}");
                                break;
                        }
                    }
                }

                if (isVaildRow)
                {
                    configJson.Add(dataItem);
                }
            }

            string jsonConfigContent = JsonConvert.SerializeObject(configJson, Formatting.Indented);

            jsonConfigContent = Regex.Unescape(jsonConfigContent);

            if (File.Exists(jsonConfigPath))
            {
                File.Delete(jsonConfigPath);
            }

            File.WriteAllText(jsonConfigPath, jsonConfigContent, Encoding.UTF8);
        }

        private static void CreateConfigScript(List<ConfigFieldInfo> fieldInfos, string excelPath)
        {
            string excelName = Path.GetFileNameWithoutExtension(excelPath);
            UMUtils.Debug.Log($"Create config script excelName: {excelName}");
            if (fieldInfos == null || fieldInfos.Count < 1) return;

            // 生成 Data 脚本
            string dataClassName = $"{CapitalizeFirstWord(excelName)}Data";
            StringBuilder scriptDataString = new StringBuilder();
            scriptDataString.AppendLine($"{ScriptTip}");
            scriptDataString.AppendLine($"using Newtonsoft.Json;");
            scriptDataString.AppendLine($"");
            scriptDataString.AppendLine($"public class {dataClassName}");
            scriptDataString.AppendLine("{");
            foreach (var cfi in fieldInfos)
            {
                if (UMUtils.String.IsValid(cfi.Comments))
                {
                    scriptDataString.AppendLine($"    /// <summary>");
                    scriptDataString.AppendLine($"    /// {cfi.Comments}");
                    scriptDataString.AppendLine($"    /// </summary>");
                }

                scriptDataString.AppendLine(
                    $"    [JsonProperty] public readonly {cfi.Type.ToLower()} {cfi.Field};");
                scriptDataString.AppendLine($"");
            }

            scriptDataString.AppendLine("}");
            string dataScriptSavePath = $"{ScriptFolder}/{dataClassName}.cs";
            if (File.Exists(dataScriptSavePath))
            {
                File.Delete(dataScriptSavePath);
            }

            File.WriteAllText(dataScriptSavePath, scriptDataString.ToString(), Encoding.UTF8);

            // 生成 Table 脚本
            StringBuilder scriptTableString = new StringBuilder();
            string tableClassName = $"{CapitalizeFirstWord(excelName)}Table";
            TableClassList.Add(tableClassName);
            scriptTableString.AppendLine($"{ScriptTip}");
            scriptTableString.AppendLine($"using System.Collections;");
            scriptTableString.AppendLine($"using System.Collections.Generic;");
            scriptTableString.AppendLine($"using Newtonsoft.Json;");
            scriptTableString.AppendLine($"using UMiniFramework.Scripts;");
            scriptTableString.AppendLine($"using UMiniFramework.Scripts.Utils;");
            scriptTableString.AppendLine($"using UMiniFramework.Scripts.Modules.ConfigModule;");
            scriptTableString.AppendLine($"using UnityEngine;");
            scriptTableString.AppendLine($"");
            scriptTableString.AppendLine($"public class {tableClassName} : UMConfigTable");
            scriptTableString.AppendLine("{");

            string configPath = DataFolder.Replace($"{Application.dataPath}/", "");
            scriptTableString.AppendLine($"    /// <summary>");
            scriptTableString.AppendLine($"    /// 配置文件路径");
            scriptTableString.AppendLine($"    /// </summary>");

            string configAssetPath = $"{configPath}/{excelName}";
            scriptTableString.AppendLine($"    public const string ConfigAssetPath = \"{configAssetPath}\";");

            string[] splitStrs = configAssetPath.Split(new[] {"Resources/"}, StringSplitOptions.None);
            string configLoadPath = splitStrs[splitStrs.Length - 1];
            scriptTableString.AppendLine($"    public const string ConfigLoadPath = \"{configLoadPath}\";");

            scriptTableString.AppendLine($"");
            scriptTableString.AppendLine($"    /// <summary>");
            scriptTableString.AppendLine($"    /// 包含在配置表中的数据");
            scriptTableString.AppendLine($"    /// </summary>");
            scriptTableString.AppendLine($"    public List<{dataClassName}> TableData {{ get; private set; }}");
            scriptTableString.AppendLine($"");

            bool isHasId = false;
            string idDicTypeString = String.Empty;

            foreach (var cfi in fieldInfos)
            {
                if (cfi.Field.ToLower() == "id")
                {
                    string idType = cfi.Type.ToLower();
                    if (idType != "string")
                    {
                        UMUtils.Debug.Error(
                            $"invalid id type in excel. path: {excelPath}; id type must use string");
                        return;
                    }

                    idDicTypeString = $"Dictionary<{idType}, {dataClassName}>";

                    scriptTableString.AppendLine($"    private {idDicTypeString} m_dataDicById;");
                    scriptTableString.AppendLine($"");
                    scriptTableString.AppendLine($"    /// <summary>");
                    scriptTableString.AppendLine($"    /// 通过 Id 属性查询数据");
                    scriptTableString.AppendLine($"    /// </summary>");
                    scriptTableString.AppendLine(
                        $"    public {dataClassName} GetDataById({idType} id)");
                    scriptTableString.AppendLine($"    {{");
                    scriptTableString.AppendLine($"        if (m_dataDicById.ContainsKey(id))");
                    scriptTableString.AppendLine($"            return m_dataDicById[id];");
                    scriptTableString.AppendLine($"        else");
                    scriptTableString.AppendLine(
                        $"            UMUtils.Debug.Warning($\"{tableClassName} id does not exist {{id}}\");");
                    scriptTableString.AppendLine($"        return null;");
                    scriptTableString.AppendLine($"    }}");
                    isHasId = true;
                }

                break;
            }

            scriptTableString.AppendLine($"");
            scriptTableString.AppendLine($"    public override IEnumerator Init()");
            scriptTableString.AppendLine($"    {{");
            if (isHasId)
            {
                scriptTableString.AppendLine($"        m_dataDicById = new {idDicTypeString}();");
            }

            scriptTableString.AppendLine($"        string jsonCofig = string.Empty;");
            scriptTableString.AppendLine(
                $"        UMini.Asset.LoadAsync<TextAsset>(ConfigLoadPath, (configData) =>{{");
            scriptTableString.AppendLine($"        if (configData != null)");
            scriptTableString.AppendLine($"        {{");
            scriptTableString.AppendLine($"            jsonCofig = configData.Resource.text;");
            scriptTableString.AppendLine(
                $"            TableData = JsonConvert.DeserializeObject<List<{dataClassName}>>(jsonCofig);");
            // scriptTableString.AppendLine($"            TableData.AsReadOnly();");
            if (isHasId)
            {
                scriptTableString.AppendLine($"            foreach (var data in TableData){{");
                scriptTableString.AppendLine($"                m_dataDicById.Add(data.id, data);");
                scriptTableString.AppendLine($"            }}");
            }

            scriptTableString.AppendLine(
                $"        UMUtils.Debug.Log($\"Init Config: {{GetType().FullName}} Succeed.\");");
            scriptTableString.AppendLine($"        }}");
            scriptTableString.AppendLine($"        else");
            scriptTableString.AppendLine($"        {{");
            scriptTableString.AppendLine(
                $"            UMUtils.Debug.Warning($\"config load failed. path: {{ConfigLoadPath}}\");");
            scriptTableString.AppendLine($"        }}}});");
            scriptTableString.AppendLine($"        yield return new WaitUntil(() => {{ return TableData != null; }});");
            scriptTableString.AppendLine($"    }}");
            scriptTableString.AppendLine("}");

            string tableScriptSavePath = $"{ScriptFolder}/{tableClassName}.cs";
            if (File.Exists(tableScriptSavePath))
            {
                File.Delete(tableScriptSavePath);
            }

            File.WriteAllText(tableScriptSavePath, scriptTableString.ToString(), Encoding.UTF8);

            // Debug.Log(scriptTableString.ToString());
        }

        private static void GetAllExcelFiles(string folderPath, List<string> excels)
        {
            // 递归获取所有 excel 文件
            DirectoryInfo d = new DirectoryInfo(folderPath);
            FileInfo[] files = d.GetFiles(); //文件
            DirectoryInfo[] directs = d.GetDirectories(); //文件夹
            foreach (FileInfo f in files)
            {
                if (f.Extension == ".xlsx" || f.Extension == ".xls")
                {
                    excels.Add(f.FullName); //添加文件名到列表中  
                }
            }

            //获取子文件夹内的文件列表，递归遍历  
            foreach (DirectoryInfo dd in directs)
            {
                GetAllExcelFiles(dd.FullName, excels);
            }
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <returns></returns>
        private static string CapitalizeFirstWord(string content)
        {
            return content.Substring(0, 1).ToUpper() + content.Substring(1);
        }
    }

    /// <summary>
    /// 配置表每个字段的信息
    /// </summary>
    public class ConfigFieldInfo
    {
        public readonly string Comments;
        public readonly string Field;
        public readonly string Type;
        public readonly int ColumnIndex;

        public ConfigFieldInfo(string comments, string field, string type, int columnIndex)
        {
            Comments = comments;
            Field = field;
            Type = type;
            ColumnIndex = columnIndex;
        }

        public override string ToString()
        {
            return $"Comments: {Comments}    Field: {Field}    Type: {Type}";
        }
    }
}