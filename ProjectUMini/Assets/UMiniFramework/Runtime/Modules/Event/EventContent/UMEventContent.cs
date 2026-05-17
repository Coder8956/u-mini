using UMiniFramework.Runtime.Modules;

namespace UMiniFramework.Runtime.Modules
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