using System;
using System.Collections;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Resource.InitArgs;
using UMiniFramework.Runtime.Modules.Resource.UMResHandlers.Interface;
using UMiniFramework.Runtime.Utils;
using Object = UnityEngine.Object;

namespace UMiniFramework.Runtime.Modules.Resource
{
    /// <summary>
    /// UI模块, 同时也是UI的根Canvas
    /// </summary>
    public class UMRes : UMBaseModule
    {
        private IUMResHandler m_resourceHandler;
        private UMResInitArgs m_initArgs = null;
        private MethodInfo m_handlerLoadMethod = null;

        public override UMModuleType ModuleType
        {
            get => UMModuleType.Resource;
        }

        private void UseDefaultInitArgs()
        {
            m_resourceHandler = UMResDIArgs.ResHandler();
        }

        private void ReadInitArgs()
        {
            m_resourceHandler = m_initArgs.ResHandler;
        }

        protected override IEnumerator Init(UMModuleInitArgs initArgs)
        {
            Type handlerType = typeof(IUMResHandler);
            m_handlerLoadMethod = UMUtilCommon.GetObjectNoPublicMethod(handlerType, "Load");

            m_initArgs = UMUtilCommon.ConvertObjectClass<UMResInitArgs>(initArgs);
            if (m_initArgs == null)
            {
                UseDefaultInitArgs();
            }
            else
            {
                ReadInitArgs();
            }

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