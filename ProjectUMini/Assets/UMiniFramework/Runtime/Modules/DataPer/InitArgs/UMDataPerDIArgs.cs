using UMiniFramework.Runtime.Modules.DataPer.UMDataPerHandlers.Interface;
using UMiniFramework.Runtime.Modules.DataPer.UMDataPerHandlers;

namespace UMiniFramework.Runtime.Modules.DataPer.InitArgs
{
    /// <summary>
    /// 数据持久化模块 默认初始化参数
    /// </summary>
    public class UMDataPerDIArgs
    {
        public static IUMDataPerHandler DataPerHandler()
        {
            return new UMDataJsonFileHandler();
        }
    }
}