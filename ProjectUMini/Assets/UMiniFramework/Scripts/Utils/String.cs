namespace UMiniFramework.Scripts.Utils
{
    public partial class UMUtils
    {
        public class String
        {
            /// <summary>
            /// 检测是否是有效的字符串
            /// </summary>
            /// <param name="content">需要检测的字符串</param>
            /// <returns></returns>
            public static bool IsValid(string content)
            {
                return (content != null && content != string.Empty && content.Length > 0);
            }
        }
    }
}