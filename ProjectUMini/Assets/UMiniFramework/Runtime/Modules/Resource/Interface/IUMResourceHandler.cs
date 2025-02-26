namespace UMiniFramework.Runtime.Modules.Resource.Interface
{
    public interface IUMResourceHandler
    {
        protected T Load<T>(string path) where T : UnityEngine.Object;
    }
}