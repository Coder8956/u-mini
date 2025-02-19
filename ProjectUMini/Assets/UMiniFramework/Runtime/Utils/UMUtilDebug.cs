using UnityEngine;

namespace UMiniFramework.Runtime.Utils
{
    public static class UMUtilDebug
    {
        private const string DEBUG_TAG = "[UM_DEBUG]";
        private static bool ENABLE = true;

        public static void Enable(bool val)
        {
            ENABLE = val;
        }

        public static void Log(object msg)
        {
            if (!ENABLE) return;
            Debug.Log(MessageAddTag(msg));
        }

        public static void Warning(object msg)
        {
            if (!ENABLE) return;
            Debug.LogWarning(MessageAddTag(msg));
        }

        public static void Error(object msg)
        {
            if (!ENABLE) return;
            Debug.LogError(MessageAddTag(msg));
        }

        private static string MessageAddTag(object msg)
        {
            return $"{DEBUG_TAG} {msg}";
        }
    }
}