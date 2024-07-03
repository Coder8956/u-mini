using UnityEngine.SceneManagement;

namespace UMiniFramework.Scripts.Modules
{
    public class SceneModule : UMModule
    {
        public override void Create()
        {
        }

        public void Load(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}