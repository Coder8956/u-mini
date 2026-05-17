using System;
using System.Reflection;
using UnityEngine;

namespace UMiniFramework.Runtime.Utils
{
    public static class UMUtilCommon
    {
        public static T CreateGameObject<T>(string name, GameObject parent) where T : Component
        {
            GameObject GO = new GameObject(name, typeof(T));
            if (parent)
            {
                GO.transform.SetParent(parent.transform);
            }

            GO.transform.localPosition = Vector3.zero;
            // Debug.Log($"The {name} is created");
            return GO.GetComponent<T>();
        }
        
        public static T CreateGameObject<T>(GameObject parent) where T : Component
        {
            string GOName = typeof(T).Name;
            return CreateGameObject<T>(GOName,parent);
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
        /// 获取对象的非公共方法
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public static MethodInfo GetObjectNoPublicMethod(Type type, string methodName)
        {
            return type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// 获取对象的非公共字段(变量)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static FieldInfo GetObjectNoPublicField(Type type, string fieldName)
        {
            return type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}