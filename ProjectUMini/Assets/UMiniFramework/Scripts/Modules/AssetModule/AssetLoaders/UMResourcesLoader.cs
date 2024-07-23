using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UMiniFramework.Scripts.Modules.AssetModule.AssetLoaders
{
    public class UMResourcesLoader : IUMAssetLoader
    {
        public void LoadAsync<T>(string path, Action<UMLoadResult<T>> onCompleted) where T : Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(path);
            request.completed += (result) =>
            {
                T res = request.asset as T;
                bool loadState = (res != null);
                onCompleted?.Invoke(new UMLoadResult<T>(loadState, res));
            };
        }
    }
}