using System;
using System.Collections;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Utils;
using Object = UnityEngine.Object;

namespace UMiniFramework.Runtime.Modules
{
    /// <summary>
    /// UI模块, 同时也是UI的根Canvas
    /// </summary>
    public class UMRes : UMBaseModule
    {
        private IUMResHandler m_resourceHandler;
        private MethodInfo m_handlerLoadMethod = null;

        public override UMModuleType ModuleType
        {
            get => UMModuleType.Resource;
        }

        protected override IEnumerator Init()
        {
            Type handlerType = typeof(IUMResHandler);
            m_resourceHandler = new UMResDefaultHandler();
            m_handlerLoadMethod = UMUtilCommon.GetObjectNoPublicMethod(handlerType, "Load");
            UMUtilDebug.Log($"{GetType().Name} Inited");

            yield return null;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Load<T>(string path) where T : Object
        {
            MethodInfo genericMethod = m_handlerLoadMethod.MakeGenericMethod(typeof(T)); // 传入泛型参数
            return (T) genericMethod.Invoke(m_resourceHandler, new object[] {path});
        }
    }
}