using UnityEngine;

namespace UMiniFramework.Runtime.Modules
{
    public class UMResLoadConfigHandler : IUMLoadConfigHandler
    {
        string IUMLoadConfigHandler.LoadConfig(string path)
        {
            return Resources.Load<TextAsset>(path).text;
        }
    }
}