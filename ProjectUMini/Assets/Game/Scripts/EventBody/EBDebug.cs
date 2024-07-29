using UMiniFramework.Runtime.Modules.EventModule;

namespace Game.Scripts.EventBody
{
    public class EBDebug : UMEventBody
    {
        public EBDebug(string content)
        {
            Str = $"Hello, this is {content}.";
        }

        public string Str = string.Empty;
    }
}