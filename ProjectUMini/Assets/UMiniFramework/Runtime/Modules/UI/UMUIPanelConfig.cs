using System;

namespace UMiniFramework.Runtime.Modules.UI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UMUIPanelConfig : Attribute
    {
        public readonly string PanelPath = null;

        public readonly int Layer = 0;

        public UMUIPanelConfig(string panelPath, int layer = 0)
        {
            PanelPath = panelPath;
            Layer = layer;
        }
    }
}