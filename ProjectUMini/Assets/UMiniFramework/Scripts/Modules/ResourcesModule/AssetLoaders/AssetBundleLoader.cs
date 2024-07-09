using System;
using Object = UnityEngine.Object;

namespace UMiniFramework.Scripts.Modules.ResourcesModule.AssetLoaders
{
    public class AssetBundleLoader : IAssetLoader
    {
        public void LoadAsync<T>(string path, Action<LoadResult<T>> onCompleted) where T : Object
        {
            throw new NotImplementedException();
        }
    }
}