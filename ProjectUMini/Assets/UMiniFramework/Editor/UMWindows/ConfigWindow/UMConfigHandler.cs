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

namespace UMiniFramework.Editor.UMWindows.ConfigWindow
{
    public class UMConfigHandler
    {
        private static string ExcelFolder;
        private static string ScriptFolder;
        private static string DataFolder;

        public static void Create(string excelFolder, string scriptFolder, string dataFolder)
        {
            ExcelFolder = excelFolder;
            ScriptFolder = scriptFolder;
            DataFolder = dataFolder;

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

            foreach (var excel in excels)
            {
                CreateConfigByExcel(excel);
            }
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

                        // foreach (var cfi in fieldInfos)
                        // {
                        //     Debug.Log(cfi.ToString());
                        // }

                        //  根据excel生成Json                        
                        CreateConfigJson(fieldInfos, table, excel);

                        // 创建配置脚本
                        CreateConfigScript(fieldInfos, excel);

                        // for (int i = 0; i < table.Rows.Count; i++)
                        // {
                        //     Debug.Log(table.Rows[i][0].ToString());
                        //     Debug.Log(table.Rows[i][1].ToString());
                        // }
                    }
                }
            }

            AssetDatabase.Refresh();
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
                                dataItem[currentKey] = currentValue.ToString();
                                break;
                            case "float":
                                dataItem[currentKey] = float.Parse(currentValue.ToString());
                                break;
                            case "int":
                                dataItem[currentKey] = int.Parse(currentValue.ToString());
                                break;
                            case "bool":
                                dataItem[currentKey] = bool.Parse(currentValue.ToString());
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

            string jsonConfigContent = JsonConvert.SerializeObject(configJson);

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
            scriptDataString.AppendLine($"// SMFramework config automatically generated, please do not modify it");
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
            scriptTableString.AppendLine($"// SMFramework config automatically generated, please do not modify it");

            scriptTableString.AppendLine($"");
            scriptTableString.AppendLine($"using System.Collections.Generic;");
            scriptTableString.AppendLine($"using SMiniFramework.Interface;");
            scriptTableString.AppendLine($"using SMiniFramework.Utils;");
            scriptTableString.AppendLine($"using Newtonsoft.Json;");
            scriptTableString.AppendLine($"using UnityEngine;");
            scriptTableString.AppendLine($"");
            scriptTableString.AppendLine($"");
            scriptTableString.AppendLine($"public class {tableClassName} : IConfigTable");
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
                    if (idType != "int" && idType != "string")
                    {
                        UMUtils.Debug.Error(
                            $"invalid id type in excel. path: {excelPath}; id type must use int or string");
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
                        $"            SMUtils.Log.Warn($\"{tableClassName} id does not exist {{id}}\");");
                    scriptTableString.AppendLine($"        return null;");
                    scriptTableString.AppendLine($"    }}");
                    isHasId = true;
                }
                else
                {
                    string funcType = CapitalizeFirstWord(cfi.Field);
                    string parName = cfi.Field;

                    // 查找所有数据
                    scriptTableString.AppendLine($"    /// <summary>");
                    scriptTableString.AppendLine($"    /// 查询所有 {funcType} 属性值相等的数据");
                    scriptTableString.AppendLine($"    /// </summary>");
                    scriptTableString.AppendLine(
                        $"    public List<{dataClassName}> GetDatasBy{funcType}({cfi.Type.ToLower()} {parName})");
                    scriptTableString.AppendLine($"    {{");
                    scriptTableString.AppendLine(
                        $"        List<{dataClassName}> datas = TableData.FindAll((e) => {{ return e.{parName} == {parName}; }});");
                    scriptTableString.AppendLine($"        if (datas == null || datas.Count == 0)");
                    scriptTableString.AppendLine(
                        $"            SMUtils.Log.Warn($\"{tableClassName} don't find any datas by {funcType}: {{{parName}}}\");");
                    scriptTableString.AppendLine($"        return datas;");
                    scriptTableString.AppendLine($"    }}");

                    // 查找单个数据
                    scriptTableString.AppendLine($"    /// <summary>");
                    scriptTableString.AppendLine($"    /// 通过 {funcType} 属性查询数据");
                    scriptTableString.AppendLine($"    /// </summary>");
                    scriptTableString.AppendLine(
                        $"    public {dataClassName} GetDataBy{funcType}({cfi.Type.ToLower()} {parName})");
                    scriptTableString.AppendLine($"    {{");
                    scriptTableString.AppendLine(
                        $"        {dataClassName} data = TableData.Find((e) => {{ return e.{parName} == {parName}; }});");
                    scriptTableString.AppendLine($"        if (data == null)");
                    scriptTableString.AppendLine(
                        $"            SMUtils.Log.Warn($\"{tableClassName} don't find any data by {funcType}: {{{parName}}}\");");
                    scriptTableString.AppendLine($"        return data;");
                    scriptTableString.AppendLine($"    }}");
                }
            }

            scriptTableString.AppendLine($"");
            scriptTableString.AppendLine($"    public void Init()");
            scriptTableString.AppendLine($"    {{");
            if (isHasId)
            {
                scriptTableString.AppendLine($"        m_dataDicById = new {idDicTypeString}();");
            }

            scriptTableString.AppendLine($"        string jsonCofig = string.Empty;");
            scriptTableString.AppendLine(
                $"        TextAsset configDataTA = SMUtils.Resources.Load<TextAsset>(ConfigLoadPath);");
            scriptTableString.AppendLine($"        if (configDataTA != null)");
            scriptTableString.AppendLine($"        {{");
            scriptTableString.AppendLine($"            jsonCofig = configDataTA.text;");
            scriptTableString.AppendLine(
                $"            TableData = JsonConvert.DeserializeObject<List<{dataClassName}>>(jsonCofig);");
            // scriptTableString.AppendLine($"            TableData.AsReadOnly();");
            if (isHasId)
            {
                scriptTableString.AppendLine($"            foreach (var data in TableData){{");
                scriptTableString.AppendLine($"                m_dataDicById.Add(data.id, data);");
                scriptTableString.AppendLine($"            }}");
            }

            // scriptTableString.AppendLine($"            SMUtils.Log.Print($\"load config from path: {{ConfigPath}}. Content:\\n:{{jsonCofig}}\");");
            scriptTableString.AppendLine($"        }}");
            scriptTableString.AppendLine($"        else");
            scriptTableString.AppendLine($"        {{");
            scriptTableString.AppendLine(
                $"            SMUtils.Log.Warn($\"config load failed. path: {{ConfigLoadPath}}\");");
            scriptTableString.AppendLine($"        }}");
            scriptTableString.AppendLine($"        SMUtils.Log.Print($\"Init Config: {{GetType().FullName}}\");");
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