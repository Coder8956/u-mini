using UnityEngine;

namespace UMiniFramework.Scripts.Kit
{
    public static class UMTool
    {
        public static T CreateGameObject<T>(string name, GameObject parent) where T : Component
        {
            GameObject moduleGo = new GameObject(name, typeof(T));
            moduleGo.transform.SetParent(parent.transform);
            moduleGo.transform.localPosition = Vector3.zero;
            UMDebug.Log($"The {name} is created");
            return moduleGo.GetComponent<T>();
        }
    }
}