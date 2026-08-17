using UMiniFramework.Runtime;
using UnityEngine;
using UnityEngine.UI;

[UMUIPanelCfg("UI/Launch/PanelLaunch")]
public class PanelLaunch : UMUIPanelBase
{
    [SerializeField] private Button m_enterGame;
    [SerializeField] private Button m_set;

    protected override void OnInitialize()
    {
        m_enterGame.onClick.AddListener(EnterGame);
        m_set.onClick.AddListener(OpenSet);
    }

    private void EnterGame()
    {
        UMOScene.Load("Game");
    }

    private void OpenSet()
    {
        DemoUI.Set.Open();
    }
}