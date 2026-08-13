using UMiniFramework.Runtime;
using UnityEngine;

public class LaunchDemo : MonoBehaviour
{
    private void Start()
    {
        UMLauncher.Work();
        UMConfig.AddTable(new TemplateTable());
        UMConfig.AddTable(new LanguageCfg());
        if (UMConfig.Local != null)
        {
            UMConfig.Local.SwitchByType(UMConfig.Local.GetOptions()[2].type);
        }
        UMScene.Load("Game");
    }

    private void Update()
    {
    }
}