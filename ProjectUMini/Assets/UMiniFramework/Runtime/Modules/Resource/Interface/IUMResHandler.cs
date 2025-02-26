namespace UMiniFramework.Runtime.Modules.Resource.Interface
{
    public interface IUMResHandler
    {
        protected T Load<T>(string path) where T : UnityEngine.Object;
    }
}