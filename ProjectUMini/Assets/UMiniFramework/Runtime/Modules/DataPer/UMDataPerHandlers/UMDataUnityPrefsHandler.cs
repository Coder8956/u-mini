using UMiniFramework.Runtime.Modules.DataPer.Interface;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.DataPer.UMDataPerHandlers
{
    public class UMDataUnityPrefsHandler : IUMDataPerHandler
    {
        void IUMDataPerHandler.Init()
        {
            
        }

        void IUMDataPerHandler.Save(string key, string val)
        {
            PlayerPrefs.SetString(key, val);
        }

        string IUMDataPerHandler.Read(string key, string defaultVal)
        {
            return PlayerPrefs.GetString(key, defaultVal);
        }

        void IUMDataPerHandler.Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        void IUMDataPerHandler.DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}