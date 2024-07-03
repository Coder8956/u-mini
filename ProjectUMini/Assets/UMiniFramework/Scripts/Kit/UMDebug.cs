using UnityEngine;

namespace UMiniFramework.Scripts.Kit
{
    public static class UMDebug
    {
        private const string DEBUG_TAG = "[UM_DEBUG]";
        private static bool Enable = true;

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

        private static string MessageAddTag(object msg)
        {
            return $"{DEBUG_TAG} {msg}";
        }
    }
}