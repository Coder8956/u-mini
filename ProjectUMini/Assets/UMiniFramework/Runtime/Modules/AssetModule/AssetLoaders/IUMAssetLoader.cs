using System;
using Object = UnityEngine.Object;

namespace UMiniFramework.Runtime.Modules.AssetModule.AssetLoaders
{
    public interface IUMAssetLoader
    {
        void LoadAsync<T>(string path, Action<UMLoadResult<T>> onCompleted) where T : Object;
    }
}