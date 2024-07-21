using System.Collections;
using UMiniFramework.Scripts.Utils;
using UnityEngine;

namespace UMiniFramework.Scripts.Modules.PersistentDataModule
{
    public class UMPersistentDataModule : UMModule
    {
        [SerializeField] private Color m_logColor = Color.yellow;
        private string m_colorStr;
        private bool m_isPersiDataToConsole = true;
        
        public override IEnumerator Init(UMini.UMiniConfig config)
        {
            m_isPersiDataToConsole = config.IsPersiDataToConsole;
            m_colorStr = ColorUtility.ToHtmlStringRGB(m_logColor);
            yield return null;
        }

        private bool IsPrintLog()
        {
            return (m_isPersiDataToConsole);
        }
    }
}