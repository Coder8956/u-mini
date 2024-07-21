using UMiniFramework.Scripts.Modules.PersistentDataModule;

namespace Game.Scripts.PersistentData
{
    public class GameData : UMPersistentData
    {
        public int Level;
        public string GameName;
        public bool Passed;
    }
}