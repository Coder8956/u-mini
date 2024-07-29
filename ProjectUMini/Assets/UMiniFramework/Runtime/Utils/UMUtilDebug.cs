using UnityEngine;

namespace UMiniFramework.Runtime.Utils
{
    public static class UMUtilDebug
    {
        private const string DEBUG_TAG = "[UM_DEBUG]";
        private static bool Enable = true;

        public static void PrintLog(bool val)
        {
            Enable = val;
        }

        public static void Log(object msg)
        {
            if (!Enable) return;
            Debug.Log(MessageAddTag(msg));
        }

        public static void Warning(object msg)
        {
            if (!Enable) return;
            Debug.LogWarning(MessageAddTag(msg));
        }

        public static void Error(object msg)
        {
            if (!Enable) return;
            Debug.LogError(MessageAddTag(msg));
        }

        private static string MessageAddTag(object msg)
        {
            return $"{DEBUG_TAG} {msg}";
        }
    }
}