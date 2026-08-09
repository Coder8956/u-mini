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
            UMConfig.Local.SwitchLocal(UMConfig.Local.GetLocalOptions()[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}