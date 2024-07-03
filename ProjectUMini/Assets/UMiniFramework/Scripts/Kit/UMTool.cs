using UnityEngine;

namespace UMiniFramework.Scripts.Kit
{
    public static class UMTool
    {
        public static T CreateGameObject<T>(GameObject parent) where T : Component
        {
            string gName = typeof(T).Name;
            GameObject moduleGo = new GameObject(gName, typeof(T));
            moduleGo.transform.SetParent(parent.transform);
            moduleGo.transform.localPosition = Vector3.zero;
            UMDebug.Log($"The {gName} is created");
            return moduleGo.GetComponent<T>();
        }
    }
}