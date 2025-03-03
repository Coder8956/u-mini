using Game.Scripts.Common;
using Game.Scripts.GameEvent;
using UMiniFramework.Runtime.Modules.Event;
using UMiniFramework.Runtime.Modules.Event.EventContent.Base;
using UMiniFramework.Runtime.Modules.Event.Listener;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI.AttributeUMUI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [UMUIPanelATB("UI/PanelGame/PanelGame")]
    public class PanelGame : UMUIPanel
    {
        [SerializeField] private Button m_btnBackMain;
        [SerializeField] private Text m_txtLevelId;
        [SerializeField] private Text m_txtGameScore;
        [SerializeField] private Text m_txtGameShootCount;
        private int m_gameScore = 0;
        private int m_gameShootCount = 0;

        private UMEventListener m_addScoreListener;
        private UMEventListener m_addShootCountListener;
        private UMEventListener m_gameAgainListener;

        public UnityAction OnBackMain { get; set; }

        protected override void OnCreatePanel()
        {
            ListenGameEvents();

            m_btnBackMain.onClick.AddListener(() =>
            {
                OnBackMain?.Invoke();
                GameGlobalFunc.BackMain();
            });
        }

        private void ListenGameEvents()
        {
            m_addShootCountListener =
                new UMEventListener(GameEventTags.AddShootCount, UMListenType.Persistent, OnAddShootCount);
            UMGR.Get<UMEvent>().AddListener(m_addShootCountListener);

            m_addScoreListener = new UMEventListener(GameEventTags.AddScore, UMListenType.Persistent, OnAddScore);
            UMGR.Get<UMEvent>().AddListener(m_addScoreListener);

            m_gameAgainListener = new UMEventListener(GameEventTags.GameAgain, UMListenType.Persistent, OnGameAgin);
            UMGR.Get<UMEvent>().AddListener(m_gameAgainListener);
        }

        private void OnAddScore(UMBaseEventContent content)
        {
            ECAddScore ecAddScore = (ECAddScore) content;
            m_gameScore += ecAddScore.AddScore;
            UpdateGameScoreUI(m_gameScore);
        }

        private void OnAddShootCount(UMBaseEventContent content)
        {
            ECAddShootCount ecAddShootCount = (ECAddShootCount) content;
            m_gameShootCount += ecAddShootCount.Num;
            UpdateGameShootCountUI(m_gameShootCount);
        }

        private void OnGameAgin(UMBaseEventContent content)
        {
            ResetUI();
        }

        private void UpdateGameScoreUI(int score)
        {
            m_txtGameScore.text = string.Format("Score: {0}", score);
        }

        private void UpdateGameShootCountUI(int count)
        {
            m_txtGameShootCount.text = string.Format("Shoot Count: {0}", count);
        }

        private void ResetUI()
        {
            m_gameScore = 0;
            UpdateGameScoreUI(m_gameScore);

            m_gameShootCount = 0;
            UpdateGameShootCountUI(m_gameShootCount);
        }

        protected override void OnDestroyPanel()
        {
        }

        protected override void OnOpenPanel()
        {
            ResetUI();
            m_txtLevelId.text = string.Format("Level Id: {0}", GameGlobalVar.SelectLevelId);
        }

        protected override void OnClosePanel()
        {
        }
    }
}