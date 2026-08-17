using UMiniFramework.Runtime;

public static class DemoUI
{
    public static PanelLaunch Launch { get; private set; }
    public static PanelSet Set { get; private set; }
    public static UMUICommonPopups Popups { get; private set; }

    public static void CreateUIObjects()
    {
        Launch = UMOUI.Create<PanelLaunch>();
        Set = UMOUI.Create<PanelSet>();
        Popups = UMOUI.Create<UMUICommonPopups>();
    }
}