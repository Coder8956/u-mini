using System;
using UMiniFramework.Runtime.Common;

namespace UMiniFramework.Runtime.Modules.UI.AttributeUMUI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UMUIPanelATB : Attribute
    {
        public readonly string LoadPath = null;

        public readonly UMResLoadType LoadType = UMResLoadType.Resources;

        public UMUIPanelATB(string panelPath, UMResLoadType pathType = UMResLoadType.Resources)
        {
            LoadPath = panelPath;
            LoadType = pathType;
        }
    }
}