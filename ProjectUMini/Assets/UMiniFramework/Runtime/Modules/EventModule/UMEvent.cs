namespace UMiniFramework.Runtime.Modules.EventModule
{
    public class UMEvent
    {
        public readonly string Type;
        public readonly UMEventBody Body;

        public UMEvent(string eventType, UMEventBody eventBody)
        {
            Type = eventType;
            Body = eventBody;
        }
    }
}