using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// 从 Launch 场景启动游戏，启动前询问是否保存当前场景。
/// </summary>
public class DemoLaunchEditor
{
    private const string LaunchScenePath = "Assets/Demo/Scenes/Launch.unity";

    [MenuItem("Demo/Launch")]
    private static void LaunchGame()
    {
        var currentScene = EditorSceneManager.GetActiveScene();

        // 场景有未保存的修改时，询问用户是否保存
        if (currentScene.isDirty)
        {
            int option = EditorUtility.DisplayDialogComplex(
                "启动游戏",
                "当前场景有未保存的修改，是否在启动前保存？",
                "保存并启动",
                "不保存启动",
                "取消");

            if (option == 2) // Cancel
                return;

            if (option == 0) // Save
            {
                EditorSceneManager.SaveScene(currentScene);
            }
        }

        // 打开 Launch 场景（如果当前不在该场景）
        if (currentScene.path != LaunchScenePath)
        {
            EditorSceneManager.OpenScene(LaunchScenePath);
        }

        // 进入运行模式
        EditorApplication.EnterPlaymode();
    }
}
