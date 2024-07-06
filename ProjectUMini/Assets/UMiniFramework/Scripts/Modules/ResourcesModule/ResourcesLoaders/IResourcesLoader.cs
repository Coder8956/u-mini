using System;

namespace UMiniFramework.Scripts.Modules.ResourcesModule.ResourcesLoaders
{
    public interface IResourcesLoader
    {
        void LoadAsync<T>(string path, Action<T> onSucceed, Action<T> fail) where T : UnityEngine.Object;
    }
}