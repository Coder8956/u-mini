using System;
using UMiniFramework.Runtime;
using UnityEngine;

public class DBtnOpen2Pop : UMDebugBtnBase
{
    protected override void OnClick()
    {
        string title = "TwoBtn";
        string content =
            "TwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtnTwoBtn";
        string btnTextL = "BtnL";
        Action callbackL = () => { Debug.Log("L-Close"); };
        string btnTextR = "BtnR";
        Action callbackR = () => { Debug.Log("R-Close"); };

        DemoUI.Popups.ShowTwoButton(title, content, btnTextL, callbackL, btnTextR, callbackR, UMOUI.UIMaxLayer);
    }
}