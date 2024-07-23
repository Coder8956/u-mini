namespace UMiniFramework.Scripts.Modules.MessageModule
{
    public interface IUMMessageListener
    {
        void UMOnReceiveMessage<T>(T messageContent) where T : UMMessageContent;
    }
}