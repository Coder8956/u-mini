using System;
using System.Collections;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.DataPer;
using UMiniFramework.Runtime.Modules.Resource.Interface;
using UMiniFramework.Runtime.Utils;

namespace UMiniFramework.Runtime.Modules.Resource
{
    /// <summary>
    /// UI模块, 同时也是UI的根Canvas
    /// </summary>
    public class UMResource : UMBaseModule
    {
        private IUMResourceHandler m_resourceHandler;
        private UMResourceInitArgs m_initArgs = null;
        private MethodInfo m_handlerLoad = null;

        public override UMModuleType ModuleType
        {
            get => UMModuleType.UI;
        }

        protected override IEnumerator Init(UMModuleInitArgs initArgs)
        {
            m_initArgs = UMUtilCommon.ConvertObjectClass<UMResourceInitArgs>(initArgs);
            if (m_initArgs.ResourceHandler == null)
            {
                UMUtilDebug.Error("m_initArgs.ResourceHandler is null");
            }

            m_resourceHandler = m_initArgs.ResourceHandler;
            Type handlerType = typeof(IUMResourceHandler);
            m_handlerLoad = UMUtilCommon.GetObjectNoPublicMethod(handlerType, "Load");
            yield return null;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return (T) m_handlerLoad.Invoke(m_resourceHandler, new object[] {path});
        }
    }
}