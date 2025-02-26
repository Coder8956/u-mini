using UMiniFramework.Runtime.Modules.Resource.Interface;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Resource.UMResourceHandlers
{
    public class UMResHandler : IUMResourceHandler
    {
        T IUMResourceHandler.Load<T>(string path)
        {
            return Resources.Load<T>(path);
        }
    }
}