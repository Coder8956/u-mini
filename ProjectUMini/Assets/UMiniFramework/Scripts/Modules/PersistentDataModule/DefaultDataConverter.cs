using System;
using UnityEngine;

namespace UMiniFramework.Scripts.Modules.PersistentDataModule
{
    public class DefaultDataConverter : IDataConverter
    {
        public T Decoder<T>(string val)
        {
            T result = default(T);
            if (result is int || result is bool || result is float || result is string)
                result = (T) Convert.ChangeType(val, typeof(T));
            else
                result = JsonUtility.FromJson<T>(val);
            return result;
        }

        public string Encoder<T>(T val)
        {
            string result = String.Empty;
            if (val is int || val is bool || val is float)
                result = val.ToString();
            else if (val is string)
                result = (val as string);
            else
                result = JsonUtility.ToJson(val);
            return result;
        }
    }
}