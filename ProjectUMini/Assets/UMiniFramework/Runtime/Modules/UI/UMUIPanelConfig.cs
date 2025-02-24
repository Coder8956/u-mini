using System;
using UMiniFramework.Runtime.Common;

namespace UMiniFramework.Runtime.Modules.UI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UMUIPanelConfig : Attribute
    {
        public readonly string LoadPath = null;

        public readonly int Layer = 0;

        public readonly UMResLoadType LoadType = UMResLoadType.Resources;

        public UMUIPanelConfig(string panelPath, int layer = 0, UMResLoadType pathType = UMResLoadType.Resources)
        {
            LoadPath = panelPath;
            Layer = layer;
            LoadType = pathType;
        }
    }
}