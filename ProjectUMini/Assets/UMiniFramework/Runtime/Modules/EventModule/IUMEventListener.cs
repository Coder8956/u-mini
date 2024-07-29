namespace UMiniFramework.Runtime.Modules.EventModule
{
    public interface IUMEventListener
    {
        void UMOnReceiveEvent(UMEvent content);
    }
}