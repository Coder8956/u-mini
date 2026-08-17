using System;
using UMiniFramework.Runtime;
using UnityEngine;

public class DBtnOpen1Pop : UMDebugBtnBase
{
    protected override void OnClick()
    {
        string title = "OneBtn";
        string content = "OneBtnOneBtn";
        string btnText = "One Btn";
        Action callback = () => { Debug.Log("OneBtn-Close"); };

        DemoUI.Popups.ShowOneButton(title, content, btnText, callback, UMOUI.UIMaxLayer);
    }
}