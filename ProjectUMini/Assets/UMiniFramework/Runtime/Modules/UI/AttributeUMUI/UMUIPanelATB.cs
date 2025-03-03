using System;
using UMiniFramework.Runtime.Common;

namespace UMiniFramework.Runtime.Modules.UI.AttributeUMUI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UMUIPanelATB : Attribute
    {
        public readonly string LoadPath = null;

        public readonly int Layer = 0;

        public readonly UMResLoadType LoadType = UMResLoadType.Resources;

        public UMUIPanelATB(string panelPath, int layer = 0, UMResLoadType pathType = UMResLoadType.Resources)
        {
            LoadPath = panelPath;
            Layer = layer;
            LoadType = pathType;
        }
    }
}