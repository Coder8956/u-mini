namespace UMiniFramework.Scripts.Modules.MessageModule
{
    public interface IMessageListener
    {
        void OnReceiveMessage<T>(T messageContent) where T : UMMessageContent;
    }
}