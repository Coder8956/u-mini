using UnityEngine;

namespace UMiniFramework.Editor.Common
{
    public class UMEditorUtils
    {
        public static string GetProjectPath()
        {
            string dataPath = Application.dataPath;
            string projectPath = dataPath.Substring(0, dataPath.LastIndexOf("/Assets"));
            return projectPath;
        }
    }
}