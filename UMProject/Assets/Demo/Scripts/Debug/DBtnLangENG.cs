using UMiniFramework.Runtime;
using UnityEngine;

public class DBtnLangENG : UMDebugBtnBase
{
    protected override void OnClick()
    {
        UMOConfig.Local.SwitchByCode("ENG");
        Debug.Log("[DBtnLangENG] Switch language: English");
    }
}
