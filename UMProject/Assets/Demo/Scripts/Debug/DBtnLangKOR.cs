using UMiniFramework.Runtime;
using UnityEngine;

public class DBtnLangKOR : UMDebugBtnBase
{
    protected override void OnClick()
    {
        UMOConfig.Local.SwitchByCode("KOR");
        Debug.Log("[DBtnLangKOR] 언어 전환: 한국인");
    }
}
