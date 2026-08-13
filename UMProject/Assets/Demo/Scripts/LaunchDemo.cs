using UMiniFramework.Runtime;
using UnityEngine;

public class LaunchDemo : MonoBehaviour
{
    private void Start()
    {
        UMLauncher.Work();
        UMOConfig.AddTable(new TemplateTable());
        UMOConfig.AddTable(new LanguageCfg());
        if (UMOConfig.Local != null)
        {
            UMOConfig.Local.SwitchByType(UMOConfig.Local.GetOptions()[2].type);
        }
        UMOScene.Load("Game");
    }

    private void Update()
    {
    }
}