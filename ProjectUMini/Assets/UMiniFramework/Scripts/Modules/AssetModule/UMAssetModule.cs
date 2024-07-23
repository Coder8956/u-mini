using System;
using System.Collections;
using UMiniFramework.Scripts.Modules.AssetModule.AssetLoaders;
using UMiniFramework.Scripts.UMEntrance;
using Object = UnityEngine.Object;

namespace UMiniFramework.Scripts.Modules.AssetModule
{
    public class UMAssetModule : UMModule
    {
        private IAssetLoader m_assetLoader;

        public override IEnumerator Init(UMiniConfig config)
        {
            m_assetLoader = config.AssetLoader;
            yield return null;
        }

        public void LoadAsync<T>(string path, Action<LoadResult<T>> onCompleted) where T : Object
        {
            m_assetLoader.LoadAsync<T>(path, onCompleted);
        }
    }
}