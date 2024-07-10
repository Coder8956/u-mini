using System;

namespace UMiniFramework.Scripts.Modules.UIModule
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UMUIPanelInfo : Attribute
    {
        public readonly string PanelPath = null;

        public readonly UMUILayer Layer = UMUILayer.Middle;

        public UMUIPanelInfo(string panelPath, UMUILayer layer = UMUILayer.Middle)
        {
            PanelPath = panelPath;
            Layer = layer;
        }
    }
}