using System;
using UMiniFramework.Runtime;
using UnityEngine;

public class DBtnOpenCDPop : UMDebugBtnBase
{
    protected override void OnClick()
    {
        string title = "CDBtn";
        string content = "CDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCDCD";
        string btnText = "One Btn";
        Action callback = () => { Debug.Log("CD-Close"); };

        DemoUI.Popups.ShowCountDown(title, content, 3, callback, "CD({0})s", UMOUI.UIMaxLayer);
    }
}