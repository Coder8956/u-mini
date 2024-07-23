namespace UMiniFramework.Scripts.Modules.MessageModule
{
    public class UMListenObject
    {
        public readonly ListenType Type;
        public readonly IMessageListener Listener;

        public UMListenObject(ListenType type, IMessageListener listener)
        {
            Type = type;
            Listener = listener;
        }
    }
}