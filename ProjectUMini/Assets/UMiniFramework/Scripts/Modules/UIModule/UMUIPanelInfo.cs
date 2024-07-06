using System;

namespace UMiniFramework.Scripts.Modules.UIModule
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UMUIPanelInfo : Attribute
    {
        public readonly string PanelPath = null;

        public UMUIPanelInfo(string panelPath)
        {
            PanelPath = panelPath;
        }
    }
}