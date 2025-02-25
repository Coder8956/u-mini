namespace UMiniFramework.Runtime.Modules.DataPer.Interface
{
    public interface IUMDataPerHandler
    {
        /// <summary>
        /// 存数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        protected void Save(string key, string val);

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultVal"></param>
        protected string Read(string key, string defaultVal);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        protected void Delete(string key);

        /// <summary>
        /// 删除所有数据
        /// </summary>
        protected void DeleteAll();
    }
}