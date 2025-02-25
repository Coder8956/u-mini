using System.Collections;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UMiniFramework.Runtime.Modules.Scene
{
    public class UMScene : UMBaseModule
    {
        public override UMModuleType ModuleType
        {
            get => UMModuleType.Scene;
        }

        protected override IEnumerator Init(UMModuleInitArgs initArgs)
        {
            yield return null;
        }

        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public AsyncOperation LoadSceneAsync(string sceneName)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
            return ao;
        }
    }
}