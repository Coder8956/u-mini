using UMiniFramework.Runtime.Modules.Config.Interface;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Config.UMLoadConfigHandlers
{
    public class ResLoadConfigHandler : ILoadConfigHandler
    {
        string ILoadConfigHandler.LoadConfig(string path)
        {
            return Resources.Load<TextAsset>(path).text;
        }
    }
}