using UMiniFramework.Runtime.Modules;

namespace Game.Scripts.GameEvent
{
    public class ECAddScore : UMBaseEventContent
    {
        public ECAddScore(int addScore)
        {
            AddScore = addScore;
        }

        public int AddScore { get; private set; }
    }
}