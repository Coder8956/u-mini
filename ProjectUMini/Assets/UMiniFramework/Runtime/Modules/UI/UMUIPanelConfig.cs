using System;
using UMiniFramework.Runtime.Common;

namespace UMiniFramework.Runtime.Modules.UI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UMUIPanelConfig : Attribute
    {
        public readonly string Path = null;

        public readonly int Layer = 0;

        public readonly UMResPathType PathType = UMResPathType.Resources;

        public UMUIPanelConfig(string panelPath, int layer = 0, UMResPathType pathType = UMResPathType.Resources)
        {
            Path = panelPath;
            Layer = layer;
            PathType = pathType;
        }
    }
}