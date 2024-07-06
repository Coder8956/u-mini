using System;
using Object = UnityEngine.Object;

namespace UMiniFramework.Scripts.Modules.ResourcesModule.ResourcesLoaders
{
    public interface IResourcesLoader
    {
        void LoadAsync<T>(string path, Action<LoadResult<T>> onCompleted) where T : Object;
    }
}