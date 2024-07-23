namespace UMiniFramework.Scripts.Modules.EventModule
{
    public interface IUMEventListener
    {
        void UMOnReceiveMessage(UMEvent content);
    }
}