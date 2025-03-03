namespace UMiniFramework.Runtime.Modules.UI.InitArgs
{
    /// <summary>
    /// UI模块 默认初始化参数
    /// </summary>
    public class UMUIDIArgs
    {
        public static bool IsCreateEventSystem()
        {
            return true;
            // return false;
        }

        public static int UILayerCount()
        {
            return 3;
            // return 6;
        }
    }
}