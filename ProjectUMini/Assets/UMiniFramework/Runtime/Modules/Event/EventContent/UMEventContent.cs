using UMiniFramework.Runtime.Modules.Event.EventContent.Base;

namespace UMiniFramework.Runtime.Modules.Event.EventContent
{
    public class UMEventContent : UMBaseEventContent
    {
        private object m_content = string.Empty;

        public UMEventContent(object content)
        {
            m_content = content;
        }

        public object Content
        {
            get => m_content;
        }
    }
}