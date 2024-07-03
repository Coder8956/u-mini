using System.Collections;
using UnityEngine.SceneManagement;

namespace UMiniFramework.Scripts.Modules
{
    public class SceneModule : UMModule
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