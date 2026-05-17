using UnityEngine;

namespace UMiniFramework.Runtime.Modules
{
    public class UMResDefaultHandler : IUMResHandler
    {
        T IUMResHandler.Load<T>(string path)
        {
            return Resources.Load<T>(path);
        }
    }
}