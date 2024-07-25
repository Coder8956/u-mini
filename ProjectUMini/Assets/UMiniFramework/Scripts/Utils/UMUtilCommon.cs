using UnityEngine;

namespace UMiniFramework.Scripts.Utils
{
    public static class UMUtilCommon
    {
        public static T CreateGameObject<T>(string name, GameObject parent) where T : Component
        {
            GameObject moduleGo = new GameObject(name, typeof(T));
            if (parent)
            {
                moduleGo.transform.SetParent(parent.transform);
            }

            moduleGo.transform.localPosition = Vector3.zero;
            Debug.Log($"The {name} is created");
            return moduleGo.GetComponent<T>();
        }

        public static void PrintModuleInitFinishedLog(string moduleName, bool finished)
        {
            Debug.Log($"{moduleName}; InitFinishedVal: {finished}");
        }
    }
}