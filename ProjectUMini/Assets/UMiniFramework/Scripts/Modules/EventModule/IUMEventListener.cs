namespace UMiniFramework.Scripts.Modules.EventModule
{
    public interface IUMEventListener
    {
        void UMOnReceiveEvent(UMEvent content);
    }
}