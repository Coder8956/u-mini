using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UMiniFramework.Scripts.Modules.ResourcesModule.AssetLoaders
{
    public class ResourcesLoader : IAssetLoader
    {
        public void LoadAsync<T>(string path, Action<LoadResult<T>> onCompleted) where T : Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(path);
            request.completed += (result) =>
            {
                T res = request.asset as T;
                onCompleted?.Invoke(new LoadResult<T>(true, res));
            };
        }
    }
}