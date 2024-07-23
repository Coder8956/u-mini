namespace UMiniFramework.Scripts.Modules.EventModule
{
    public class UMListenerInfo
    {
        public readonly UMListenType Type;
        public readonly IUMEventListener Listener;

        public UMListenerInfo(UMListenType type, IUMEventListener listener)
        {
            Type = type;
            Listener = listener;
        }
    }
}