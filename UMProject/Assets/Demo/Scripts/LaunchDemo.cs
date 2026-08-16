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
        
        if (UMOConfig.Local != null)
        {
            UMOConfig.Local.SwitchByType(UMOConfig.Local.GetOptions()[0].type);
        }

        // UMOScene.Load("Game");
    }

    private void Update()
    {
        // TestSwitchLang();
    }

    private void TestSwitchLang()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.digit0Key.wasPressedThisFrame)
        {
            Debug.Log("1");
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