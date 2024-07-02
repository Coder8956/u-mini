using UMiniFramework.Scripts.Kit;
using UnityEngine;

namespace UMiniFramework.Scripts.Modules
{
    public abstract class UMModule : MonoBehaviour
    {
        // public abstract void Create();

        public static void CreateModule<T>(ref T instance, UMEntity entity) where T : UMModule
        {
            if (instance == null)
            {
                GameObject moduleGo = new GameObject(typeof(T).Name, typeof(T));
                moduleGo.transform.SetParent(entity.transform);
                moduleGo.transform.localPosition = Vector3.zero;
                instance = moduleGo.GetComponent<T>();
                UMDebug.Log($"The {moduleGo.name} is created");
            }
            else
            {
                UMDebug.Warning("Invalid module creation. Because the instance already exists");
            }
        }
    }
}