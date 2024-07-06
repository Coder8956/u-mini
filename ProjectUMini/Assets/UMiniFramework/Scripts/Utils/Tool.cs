using UnityEngine;

namespace UMiniFramework.Scripts.Utils
{
    public partial class UMUtils
    {
        public static class Tool
        {
            public static T CreateGameObject<T>(string name, GameObject parent) where T : Component
            {
                GameObject moduleGo = new GameObject(name, typeof(T));
                moduleGo.transform.SetParent(parent.transform);
                moduleGo.transform.localPosition = Vector3.zero;
                Debug.Log($"The {name} is created");
                return moduleGo.GetComponent<T>();
            }
        }
    }
}