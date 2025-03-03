using UMiniFramework.Runtime.Modules.Event.EventContent.Base;

namespace UMiniFramework.Runtime.Modules.Event.EventContent
{
    public class UMEventContent : UMBaseEventContent
    {
        private string m_content = string.Empty;

        public string Content
        {
            get => m_content;
            set => m_content = value;
        }
    }
}