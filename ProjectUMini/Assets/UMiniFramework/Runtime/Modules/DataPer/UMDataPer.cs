using System;
using System.Collections;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.DataPer;
using UMiniFramework.Runtime.Modules.DataPer.Interface;
using UMiniFramework.Runtime.Utils;

namespace UMiniFramework.Runtime.Modules.UMDataPer
{
    public class UMDataPer : UMBaseModule
    {
        private IUMDataPerHandler m_dataPerHandler;
        private UMDataPerConfig m_config = null;

        private MethodInfo m_saveMethod = null;
        private MethodInfo m_readMethod = null;
        private MethodInfo m_deleteMethod = null;
        private MethodInfo m_deleteAllMethod = null;

        public override UMModuleType ModuleType
        {
            get => UMModuleType.UMDataPer;
        }

        protected override IEnumerator Init(UMModuleConfig config)
        {
            m_config = UMUtilCommon.ConvertObjectClass<UMDataPerConfig>(config);
            if (m_config.DataPerHandler == null)
            {
                UMUtilDebug.Error("m_config.DataPerHandler is null");
            }

            m_dataPerHandler = m_config.DataPerHandler;

            Type dataPerHandlerType = typeof(IUMDataPerHandler);
            // UMUtilDebug.Log($"dataPerHandler name: {dataPerHandlerType.Name}");
            m_saveMethod = UMUtilCommon.GetObjectNoPublicMethod(dataPerHandlerType, "Save");
            m_readMethod = UMUtilCommon.GetObjectNoPublicMethod(dataPerHandlerType, "Read");
            m_deleteMethod = UMUtilCommon.GetObjectNoPublicMethod(dataPerHandlerType, "Delete");
            m_deleteAllMethod = UMUtilCommon.GetObjectNoPublicMethod(dataPerHandlerType, "DeleteAll");

            yield return null;
        }

        /// <summary>
        /// 存数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Save(string key, string val)
        {
            m_saveMethod.Invoke(m_dataPerHandler, new object[] {key, val});
        }

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultVal"></param>
        public string Read(string key, string defaultVal)
        {
            return (string) m_readMethod.Invoke(m_dataPerHandler, new object[] {key, defaultVal});
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            m_deleteMethod.Invoke(m_dataPerHandler, new object[] {key});
        }

        /// <summary>
        /// 删除所有数据
        /// </summary>
        public void DeleteAll()
        {
            m_deleteAllMethod.Invoke(m_dataPerHandler, null);
        }
    }
}