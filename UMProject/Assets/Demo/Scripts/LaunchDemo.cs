using UMiniFramework.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchDemo : MonoBehaviour
{
    private void Start()
    {
        UMLauncher.Work();

        UMOConfig.AddTable(new LanguageCfg());
        UMOConfig.AddTable(new GunTable());
        UMOConfig.AddTable(new BulletTable());
        UMOConfig.AddTable(new MonsterTable());

        UMOAudio.SFX.AddClip(new UMACInfo(DMAudio.SFX_GF, "Audio/SFX/GunFire"));
        UMOAudio.SFX.AddClip(new UMACInfo(DMAudio.SFX_TB, "Audio/SFX/TouchBtn"));

        UMOAudio.BGM.AddClip(new UMACInfo(DMAudio.BGM_Launch, "Audio/BGM/bgm01"));
        UMOAudio.BGM.AddClip(new UMACInfo(DMAudio.BGM_Game, "Audio/BGM/bgm02"));

        DemoUI.CreateUIObjects();

        // 创建 Demo 实例
        DemoEntity.Create();
        
        DemoUI.Launch.Open();

        UMOAudio.BGM.Volume = 0.1f;
        UMOAudio.BGM.Play(DMAudio.BGM_Launch);

        UMOUI.EnableDebugPanel = true;
        
        Debug.Log("Game Launched!");
    }

    private void OnDestroy()
    {
        DemoUI.Launch.Close();
    }

    private void Update()
    {
        // EnterGame();
        // TestSwitchLang();
        // TestGlobalVal();
    }

    void EnterGame()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("EnterGame");
            UMOScene.Load("Game");
        }
    }

    void TestGlobalVal()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.digit0Key.wasPressedThisFrame)
        {
            Debug.Log("TestGlobalVal");
            UMOGlobalVal.Set(DMGlobalVal.SelectGunID, "111");
        }

        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            UMOGlobalVal.Set(DMGlobalVal.SelectGunID, null);
        }

        if (keyboard.digit2Key.wasPressedThisFrame)
        {
            UMOGlobalVal.Set(DMGlobalVal.SelectGunID, 10);
        }

        if (keyboard.digit3Key.wasPressedThisFrame)
        {
            UMOGlobalVal.Set(DMGlobalVal.SelectGunID, 1.5f);
        }
    }

    private void TestSwitchLang()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.digit0Key.wasPressedThisFrame)
        {
            Debug.Log("TestSwitchLang");
            UMOConfig.Local.SwitchByType(UMOConfig.Local.GetOptions()[0].type);
        }

        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            UMOConfig.Local.SwitchByType(UMOConfig.Local.GetOptions()[1].type);
        }

        if (keyboard.digit2Key.wasPressedThisFrame)
        {
            UMOConfig.Local.SwitchByType(UMOConfig.Local.GetOptions()[2].type);
        }

        if (keyboard.digit3Key.wasPressedThisFrame)
        {
            UMOConfig.Local.SwitchByType(UMOConfig.Local.GetOptions()[3].type);
        }
    }
}