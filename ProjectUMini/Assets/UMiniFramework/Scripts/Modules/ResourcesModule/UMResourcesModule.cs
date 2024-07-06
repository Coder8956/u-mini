using System;
using System.Collections;
using UMiniFramework.Scripts.Modules.ResourcesModule.ResourcesLoaders;
using Object = UnityEngine.Object;

namespace UMiniFramework.Scripts.Modules.ResourcesModule
{
    public class UMResourcesModule : UMModule
    {
        private IResourcesLoader m_resourcesLoader;

        public override IEnumerator Init(UMini.UMiniConfig config)
        {
            yield return null;
        }

        public void LoadAsync<T>(string path, Action<T> onSucceed, Action<T> fail) where T : Object
        {
            m_resourcesLoader.LoadAsync<T>(path, onSucceed, fail);
        }
    }
}