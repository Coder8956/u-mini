namespace UMiniFramework.Scripts.Modules.PersistentDataModule
{
    public interface IDataPersistenceHandler
    {
        string Read(string key, string defaultVal);
        void Write(string key, string val);
        void Delete(string key);
        void Clear();
    }
}