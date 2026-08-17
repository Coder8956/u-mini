using UMiniFramework.Runtime;
using UnityEngine;

public class DBtnGun1 : UMDebugBtnBase
{
    protected override void OnClick()
    {
        UMOGlobalVal.Set(DMGlobalVal.SelectGunID, "gun_001");
        Debug.Log("[DBtnGun1] 切换武器ID: gun_001");
    }
}
