using System.Collections;
using UnityEngine.SceneManagement;

namespace UMiniFramework.Scripts.Modules.Scene
{
    public class UMSceneModule : UMModule
    {
        public override IEnumerator Init()
        {
            yield return null;
        }

        public void Load(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}