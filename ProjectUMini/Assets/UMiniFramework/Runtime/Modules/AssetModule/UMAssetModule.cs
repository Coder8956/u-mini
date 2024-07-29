using System;
using System.Collections;
using UMiniFramework.Runtime.Modules.AssetModule.AssetLoaders;
using UMiniFramework.Runtime.UMEntrance;
using UMiniFramework.Runtime.Utils;
using Object = UnityEngine.Object;

namespace UMiniFramework.Runtime.Modules.AssetModule
{
    public class UMAssetModule : UMModule
    {
        private IUMAssetLoader m_assetLoader;

        public override IEnumerator Init(UMiniConfig config)
        {
            m_assetLoader = config.AssetLoader;
            yield return null;
            m_initFinished = true;
            UMUtilCommon.PrintModuleInitFinishedLog(GetType().Name, m_initFinished);
        }

        public void LoadAsync<T>(string path, Action<UMLoadResult<T>> onCompleted) where T : Object
        {
            m_assetLoader.LoadAsync<T>(path, onCompleted);
        }
    }
}