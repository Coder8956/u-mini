using UMiniFramework.Runtime;
using UnityEngine;

public class DBtnLangSC : UMDebugBtnBase
{
    protected override void OnClick()
    {
        UMOConfig.Local.SwitchByCode("SC");
        Debug.Log("[DBtnLangSC] 切换语言: 简体中文");
    }
}
