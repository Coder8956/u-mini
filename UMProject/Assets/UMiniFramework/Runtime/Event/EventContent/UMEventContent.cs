namespace UMiniFramework.Runtime
{
    public class UMEventContent : UMBaseEventContent
    {
        public UMEventContent(object content)
        {
            Content = content;
        }

        public object Content { get; }
    }
}