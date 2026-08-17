using UMiniFramework.Runtime;
using UnityEngine;

public class DBtnLangJPN : UMDebugBtnBase
{
    protected override void OnClick()
    {
        UMOConfig.Local.SwitchByCode("JPN");
        Debug.Log("[DBtnLangJPN] 言語切替: 日本語");
    }
}
