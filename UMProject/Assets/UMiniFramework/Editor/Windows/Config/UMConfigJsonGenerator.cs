using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UMiniFramework.Editor
{
    internal static class UMConfigJsonGenerator
    {
        private const string ARR_SPLIT = ";";

        public static void CreateConfigJson(
            List<ConfigFieldInfo> fieldInfos,
            DataTable table,
            string excelPath,
            string dataFolder)
        {
            if (fieldInfos == null || fieldInfos.Count < 1) return;

            string excelName = Path.GetFileNameWithoutExtension(excelPath);
            string jsonConfigPath = $"{dataFolder}/{excelName}.json";

            var configJson = new JArray();

            // Rows starting from index 3 are data rows
            for (var i = 3; i < table.Rows.Count; i++)
            {
                // Skip rows where the first column is empty
                if (string.IsNullOrEmpty(table.Rows[i][0].ToString()))
                    continue;

                var dataItem = new JObject();
                foreach (var cfi in fieldInfos)
                {
                    string currentValue = table.Rows[i][cfi.ColumnIndex]
                        .ToString().Replace("\"", "\\\"");

                    AddFieldValue(dataItem, cfi.Field, cfi.Type.ToLower(), currentValue);
                }

                configJson.Add(dataItem);
            }

            string jsonConfigContent = JsonConvert.SerializeObject(configJson, Formatting.Indented);
            jsonConfigContent = Regex.Unescape(jsonConfigContent);

            File.WriteAllText(jsonConfigPath, jsonConfigContent, Encoding.UTF8);
        }

        private static void AddFieldValue(JObject dataItem, string key, string type, string value)
        {
            switch (type)
            {
                case "string":
                    dataItem[key] = value;
                    break;
                case "string[]":
                    dataItem[key] = ParseArray(value, s => s);
                    break;
                case "float":
                    dataItem[key] = float.Parse(value);
                    break;
                case "float[]":
                    dataItem[key] = ParseArray(value, float.Parse);
                    break;
                case "int":
                    dataItem[key] = int.Parse(value);
                    break;
                case "int[]":
                    dataItem[key] = ParseArray(value, int.Parse);
                    break;
                case "bool":
                    dataItem[key] = bool.Parse(value);
                    break;
                case "bool[]":
                    dataItem[key] = ParseArray(value, bool.Parse);
                    break;
            }
        }

        private static JArray ParseArray<T>(string value, Func<string, T> parser)
        {
            var jArray = new JArray();
            string[] parts = value.Split(ARR_SPLIT);
            foreach (var s in parts)
            {
                if (!string.IsNullOrEmpty(s))
                    jArray.Add(parser(s));
            }

            return jArray;
        }

        public static void CreateLangJson(
            DataTable table,
            string dataFolder)
        {
            string langDir = $"{dataFolder}/lang";
            Directory.CreateDirectory(langDir);

            // Row 0: comments row — Column 0 is "语言id", Column 1+ are language display names
            DataRow commentsRow = table.Rows[0];
            int totalCols = table.Columns.Count;

            var langTypes = new JArray();
            for (int col = 1; col < totalCols; col++)
            {
                string langName = commentsRow[col].ToString();
                if (string.IsNullOrEmpty(langName))
                    break;
                langTypes.Add(langName);
            }

            int langCount = langTypes.Count;

            // Write lang_types.json
            string typesJson = JsonConvert.SerializeObject(langTypes, Formatting.Indented);
            File.WriteAllText($"{langDir}/lang_types.json", typesJson, Encoding.UTF8);

            // Row 3+: data rows — Column 0 = id, Column (colIndex + 1) = content for language at colIndex
            for (int colIndex = 0; colIndex < langCount; colIndex++)
            {
                int col = colIndex + 1;
                var langContent = new JObject();

                for (int row = 3; row < table.Rows.Count; row++)
                {
                    string id = table.Rows[row][0].ToString();
                    if (string.IsNullOrEmpty(id))
                        continue;

                    string content = table.Rows[row][col].ToString();
                    langContent[id] = content;
                }

                string json = JsonConvert.SerializeObject(langContent, Formatting.Indented);
                File.WriteAllText($"{langDir}/lang_{colIndex}.json", json, Encoding.UTF8);
            }
        }
    }
}
