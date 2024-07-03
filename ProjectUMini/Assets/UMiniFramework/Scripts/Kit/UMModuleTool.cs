using UMiniFramework.Scripts.Modules;
using UnityEngine;

namespace UMiniFramework.Scripts.Kit
{
    public class UMModuleTool
    {
        public static T CreateModule<T>(UMEntity entity) where T : UMModule
        {
            string moduleName = typeof(T).Name;
            GameObject moduleGo = new GameObject(moduleName, typeof(T));
            moduleGo.transform.SetParent(entity.transform);
            moduleGo.transform.localPosition = Vector3.zero;
            UMDebug.Log($"The {moduleName} is created");
            return moduleGo.GetComponent<T>();
        }
    }
}