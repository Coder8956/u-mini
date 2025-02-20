using System;

namespace UMiniFramework.Runtime.Modules.UI
{
    public enum PathEnum
    {
        Resources,
        AssetBundle
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class UMUIPanelConfig : Attribute
    {
        public readonly string Path = null;

        public readonly int Layer = 0;

        public readonly PathEnum PathType = PathEnum.Resources;

        public UMUIPanelConfig(string panelPath, int layer = 0, PathEnum pathType = PathEnum.Resources)
        {
            Path = panelPath;
            Layer = layer;
            PathType = pathType;
        }
    }
}