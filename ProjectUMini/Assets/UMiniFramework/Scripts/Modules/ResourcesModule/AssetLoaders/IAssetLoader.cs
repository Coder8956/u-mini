using System;
using Object = UnityEngine.Object;

namespace UMiniFramework.Scripts.Modules.ResourcesModule.AssetLoaders
{
    
    public interface IAssetLoader
    {
        void LoadAsync<T>(string path, Action<LoadResult<T>> onCompleted) where T : Object;
    }
}