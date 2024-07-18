using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor.CommonUtils.Scene
{
    public class EditorSceneUtils
    {
        public class SceneExtend
        {
            private const string LAUNCHER_SCENE = "Assets/Game/Scenes/Game/Launch.unity";

            [MenuItem("TFGUtils/Scene/OpenLauncherScene")]
            private static void OpenLauncherScene()
            {
                OpenScene(LAUNCHER_SCENE);
            }
            
            [MenuItem("TFGUtils/Scene/OpenGameScene")]
            private static void OpenGameScene()
            {
                OpenScene("Assets/Game/Scenes/Game/Game.unity");
            }

            private static void OpenScene(string scenePath)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(scenePath);
            }

            [MenuItem("TFGUtils/Scene/OpenLauncherSceneAndPlay")]
            private static void PlayGame()
            {
                if (EditorApplication.isPlaying) return;

                OpenScene(LAUNCHER_SCENE);

                if (!EditorApplication.isPlaying)
                    EditorApplication.isPlaying = true;
            }
        }
    }
}
//
// public static class EditorTools
// {
//     [MenuItem("Assets/GameStart #F5", false, 999)] // 使用快捷键Shift+F5进入或退出Play模式
//     static void PlayGame()
//     {
//         if (EditorApplication.isPlaying) return;
//
//         var mainScene = "Assets/Scenes/Init.unity";
//
//         if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
//             EditorSceneManager.OpenScene(mainScene);
//
//         if (!EditorApplication.isPlaying)
//             EditorApplication.isPlaying = true;
//     }
//
//     [MenuItem("Assets/Open Hotfix Sln #F9", false, 998)]
//     public static void OpenHotfixSln()
//     {
//         var path = Directory.GetParent(Application.dataPath) + "\\HotUpdateScripts\\HotUpdateScripts.sln";
//         System.Diagnostics.Process.Start(path);
//     }
// }