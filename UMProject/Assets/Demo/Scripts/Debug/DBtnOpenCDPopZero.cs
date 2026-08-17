using System;
using UMiniFramework.Runtime;
using UnityEngine;

public class DBtnOpenCDPopZero : UMDebugBtnBase
{
    protected override void OnClick()
    {
        string title = "CDBtn";
        string content = "CDCD000000000000000000000000000000000000000000000000";
        string btnText = "One Btn";
        Action callback = () => { Debug.Log("CD ZERO -Close"); };

        DemoUI.Popups.ShowCountDown(title, content, 0, callback, "CD({0})s", UMOUI.UIMaxLayer);
    }
}