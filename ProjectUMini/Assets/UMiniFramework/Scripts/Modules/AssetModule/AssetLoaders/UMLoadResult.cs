namespace UMiniFramework.Scripts.Modules.AssetModule.AssetLoaders
{
    public class UMLoadResult<T> where T : UnityEngine.Object
    {
        /// <summary>
        /// 值为true资源加载成功
        /// </summary>
        public readonly bool State = false;

        /// <summary>
        /// 获取到的资源
        /// </summary>
        public readonly T Resource = null;

        public UMLoadResult(bool loadState, T loadResource)
        {
            State = loadState;
            Resource = loadResource;
        }
    }
}