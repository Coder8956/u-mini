using System.Collections;
using UnityEngine.SceneManagement;

namespace UMiniFramework.Scripts.Modules.SceneModule
{
    public class UMSceneModule : UMModule
    {
        public override IEnumerator Init(UMini.UMiniConfig config)
        {
            yield return null;
        }

        public void Load(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}