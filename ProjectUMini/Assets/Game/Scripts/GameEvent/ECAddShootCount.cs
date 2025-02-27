using UMiniFramework.Runtime.Modules.Event.Base;

namespace Game.Scripts.GameEvent
{
    public class ECAddShootCount : UMBaseEventContent
    {
        public ECAddShootCount(int num)
        {
            Num = num;
        }

        public int Num { get; private set; }
    }
}