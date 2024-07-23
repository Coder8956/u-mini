namespace UMiniFramework.Scripts.Modules.MessageModule
{
    public interface IUMMessageListener
    {
        void UMOnReceiveMessage(UMMessageContent messageContent);
    }
}