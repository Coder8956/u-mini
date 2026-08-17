using UMiniFramework.Runtime;
using UnityEngine;

public class DBtnGun2 : UMDebugBtnBase
{
    protected override void OnClick()
    {
        UMOGlobalVal.Set(DMGlobalVal.SelectGunID, "gun_002");
        Debug.Log("[DBtnGun2] 切换武器ID: gun_002");
    }
}
