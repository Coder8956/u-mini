using UMiniFramework.Runtime.Modules.Event.EventContent.Base;

namespace UMiniFramework.Runtime.Modules.Event.EventContent
{
    public class UMEventContent : UMBaseEventContent
    {
        private string m_content = string.Empty;

        public UMEventContent(string content)
        {
            m_content = content;
        }

        public string Content
        {
            get => m_content;
        }
    }
}