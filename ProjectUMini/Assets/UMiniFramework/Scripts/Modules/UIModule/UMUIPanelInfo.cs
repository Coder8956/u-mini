using System;

namespace UMiniFramework.Scripts.Modules.UIModule
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UMUIPanelInfo : Attribute
    {
        public readonly string PanelPath = null;
        public readonly bool Single = true;

        public UMUIPanelInfo(string panelPath, bool single = true)
        {
            PanelPath = panelPath;
            Single = true;
        }
    }
}