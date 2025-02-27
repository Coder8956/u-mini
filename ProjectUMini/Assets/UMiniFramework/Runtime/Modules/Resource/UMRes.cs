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
    public class UMRes : UMBaseModule
    {
        private IUMResHandler m_resourceHandler;
        private UMResInitArgs m_initArgs = null;
        private MethodInfo m_handlerLoad = null;

        public override UMModuleType ModuleType
        {
            get => UMModuleType.Resource;
        }

        protected override IEnumerator Init(UMModuleInitArgs initArgs)
        {
            m_initArgs = UMUtilCommon.ConvertObjectClass<UMResInitArgs>(initArgs);
            if (m_initArgs.ResHandler == null)
            {
                UMUtilDebug.Error("m_initArgs.ResourceHandler is null");
            }

            m_resourceHandler = m_initArgs.ResHandler;
            Type handlerType = typeof(IUMResHandler);
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
            MethodInfo genericMethod = m_handlerLoad.MakeGenericMethod(typeof(T)); // 传入泛型参数
            return (T) genericMethod.Invoke(m_resourceHandler, new object[] {path});
        }
    }
}