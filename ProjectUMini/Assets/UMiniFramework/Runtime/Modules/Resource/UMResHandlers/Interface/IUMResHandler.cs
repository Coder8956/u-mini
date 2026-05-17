using UnityEngine;

namespace UMiniFramework.Runtime.Modules
{
    public interface IUMResHandler
    {
        protected T Load<T>(string path) where T : Object;
    }
}