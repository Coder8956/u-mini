using UMiniFramework.Runtime;
using UnityEngine;

public class DBtnLaunch : UMDebugBtnBase
{
    protected override void OnClick()
    {
        UMOScene.Load("Launch");
        Debug.Log("[DBtnLaunch] 进入游戏场景: Launch");
    }
}
