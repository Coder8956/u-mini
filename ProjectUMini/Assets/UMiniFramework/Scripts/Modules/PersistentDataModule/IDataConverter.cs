namespace UMiniFramework.Scripts.Modules.PersistentDataModule
{
    public interface IDataConverter
    {
        T Decoder<T>(string val);
        string Encoder<T>(T val);
    }
}