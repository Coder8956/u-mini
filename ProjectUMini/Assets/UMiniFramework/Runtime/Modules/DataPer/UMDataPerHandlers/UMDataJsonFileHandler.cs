using UMiniFramework.Runtime.Modules.DataPer.Interface;

namespace UMiniFramework.Runtime.Modules.DataPer.UMDataPerHandlers
{
    public class UMDataJsonFileHandler : IUMDataPerHandler
    {
        void IUMDataPerHandler.Save(string key, string val)
        {
            throw new System.NotImplementedException();
        }

        string IUMDataPerHandler.Read(string key, string defaultVal)
        {
            throw new System.NotImplementedException();
        }

        void IUMDataPerHandler.Delete(string key)
        {
            throw new System.NotImplementedException();
        }

        void IUMDataPerHandler.DeleteAll()
        {
            throw new System.NotImplementedException();
        }
    }
}