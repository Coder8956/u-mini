using UMiniFramework.Runtime;

public class DBtnOpenClose : UMDebugBtnBase
{
    protected override void OnClick()
    {
        DemoUI.Popups.Close();
    }
}