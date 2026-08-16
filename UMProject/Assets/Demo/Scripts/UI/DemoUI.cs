using UMiniFramework.Runtime;

public static class DemoUI
{
    public static PanelLaunch Launch { get; private set; }

    public static void CreateUIObjects()
    {
        Launch = UMOUI.Create<PanelLaunch>();
    }
}