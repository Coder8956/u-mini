using System;
using Object = UnityEngine.Object;

namespace UMiniFramework.Runtime.Modules.AssetModule.AssetLoaders
{
    public class UMAssetBundleLoader : IUMAssetLoader
    {
        public void LoadAsync<T>(string path, Action<UMLoadResult<T>> onCompleted) where T : Object
        {
            throw new NotImplementedException();
        }
    }
}