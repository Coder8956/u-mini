namespace UMiniFramework.Scripts.Modules.MessageModule
{
    public class UMListenObject
    {
        public readonly UMListenType Type;
        public readonly IUMMessageListener Listener;

        public UMListenObject(UMListenType type, IUMMessageListener listener)
        {
            Type = type;
            Listener = listener;
        }
    }
}