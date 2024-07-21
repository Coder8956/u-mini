using UnityEngine;

namespace UMiniFramework.Scripts.Modules.PersistentDataModule
{
    public class UMDefaultDataPers : IDataPersistenceHandler
    {
        public string Read(string key, string defaultVal)
        {
            return PlayerPrefs.GetString(key, defaultVal);
        }

        public void Write(string key, string val)
        {
            PlayerPrefs.SetString(key, val);
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public void Clear()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}