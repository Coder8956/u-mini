using System;
using System.Reflection;
using UnityEngine;

namespace UMiniFramework.Runtime.Utils
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
            // Debug.Log($"The {name} is created");
            return moduleGo.GetComponent<T>();
        }

        /// <summary>
        /// 转换对象类型(class)
        /// </summary>
        /// <param name="obj">对象</param>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>转换失败返回 null</returns>
        public static T ConvertObjectClass<T>(object obj) where T : class
        {
            if (obj is T)
                return (T) obj;
            else
                return null;
        }

        /// <summary>
        /// 获取非公共方法的对象方法
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public static MethodInfo GetObjectNoPublicMethod(Type type, string methodName)
        {
            return type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}