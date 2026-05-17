using System;

namespace UMiniFramework.Runtime.Modules.Config
{
    public static class UMConfigUtils
    {
        /// <summary>
        /// 切分配置字符串的键值对
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        // public static UMCfgKV<K, V> SplitKV<K, V>(string content)
        // {
        //     return new(content);
        // }
    }

    /// <summary>
    /// 解析由 ":" 分割的字符串键值对
    /// K, V 只支持 string; bool; int; uint; float;
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class UMCfgKV<K, V>
    {
        public readonly K Key;
        public readonly V Value;

        public UMCfgKV(string content)
        {
            string[] kv = content.Split(':');
            Key = ConvertTo<K>(kv[0]);
            Value = ConvertTo<V>(kv[1]);
        }

        private static T ConvertTo<T>(string value)
        {
            Type type = typeof(T);

            // 处理 string
            if (type == typeof(string))
            {
                return (T) (object) value;
            }

            // 处理 enum
            // if (type.IsEnum)
            // {
            //     return (T)Enum.Parse(type, value);
            // }


            // 常见数值类型（更安全 & 性能更好）
            if (type == typeof(int))
                return (T) (object) int.Parse(value);

            if (type == typeof(uint))
                return (T) (object) uint.Parse(value);

            if (type == typeof(float))
                return (T) (object) float.Parse(value);

            // if (type == typeof(double))
            //     return (T)(object)double.Parse(value);

            if (type == typeof(bool))
                return (T) (object) bool.Parse(value);

            // if (type == typeof(long))
            //     return (T)(object)long.Parse(value);
            //
            // if (type == typeof(ulong))
            //     return (T)(object)ulong.Parse(value);

            // 兜底方案
            return (T) Convert.ChangeType(value, type);
        }
    }
}