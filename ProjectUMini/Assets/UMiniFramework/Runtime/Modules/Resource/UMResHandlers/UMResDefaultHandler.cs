using UMiniFramework.Runtime.Modules.Resource.UMResHandlers.Interface;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Resource.UMResHandlers
{
    public class UMResDefaultHandler : IUMResHandler
    {
        T IUMResHandler.Load<T>(string path)
        {
            return Resources.Load<T>(path);
        }
    }
}