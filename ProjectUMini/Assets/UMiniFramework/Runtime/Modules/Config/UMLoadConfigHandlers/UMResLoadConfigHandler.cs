using UMiniFramework.Runtime.Modules.Config.UMLoadConfigHandlers.Interface;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Config.UMLoadConfigHandlers
{
    public class UMResLoadConfigHandler : ILoadConfigHandler
    {
        string ILoadConfigHandler.LoadConfig(string path)
        {
            return Resources.Load<TextAsset>(path).text;
        }
    }
}