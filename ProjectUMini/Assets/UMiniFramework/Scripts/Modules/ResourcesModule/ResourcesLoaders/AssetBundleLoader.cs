using System;
using Object = UnityEngine.Object;

namespace UMiniFramework.Scripts.Modules.ResourcesModule.ResourcesLoaders
{
    public class AssetBundleLoader : IResourcesLoader
    {
        public void LoadAsync<T>(string path, Action<LoadResult<T>> onCompleted) where T : Object
        {
            throw new NotImplementedException();
        }
    }
}