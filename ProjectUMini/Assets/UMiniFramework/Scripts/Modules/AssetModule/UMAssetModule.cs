using System;
using System.Collections;
using UMiniFramework.Scripts.Modules.AssetModule.AssetLoaders;
using Object = UnityEngine.Object;

namespace UMiniFramework.Scripts.Modules.AssetModule
{
    public class UMAssetModule : UMModule
    {
        private IAssetLoader m_assetLoader;

        public override IEnumerator Init(UMini.UMiniConfig config)
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