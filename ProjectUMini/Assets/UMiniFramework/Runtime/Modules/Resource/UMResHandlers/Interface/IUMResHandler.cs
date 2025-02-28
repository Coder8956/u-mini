using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Resource.UMResHandlers.Interface
{
    public interface IUMResHandler
    {
        protected T Load<T>(string path) where T : Object;
    }
}