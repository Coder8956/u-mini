using System;
using Object = UnityEngine.Object;

namespace UMiniFramework.Scripts.Modules.ResourcesModule.ResourcesLoaders
{
    public class ResourcesLoader : IResourcesLoader
    {
        public void LoadAsync<T>(string path, Action<T> onSucceed, Action<T> fail) where T : Object
        {
            throw new NotImplementedException();
        }
    }
}