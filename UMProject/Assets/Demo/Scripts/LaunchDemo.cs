using UMiniFramework.Runtime;
using UnityEngine;

public class LaunchDemo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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

    // Update is called once per frame
    void Update()
    {
    }
}