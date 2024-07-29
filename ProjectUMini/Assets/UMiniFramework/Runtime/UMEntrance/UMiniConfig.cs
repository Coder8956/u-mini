using System;
using System.Collections.Generic;
using UMiniFramework.Runtime.Modules.AssetModule.AssetLoaders;
using UMiniFramework.Runtime.Modules.ConfigModule;
using UMiniFramework.Runtime.Modules.PersistentDataModule;

namespace UMiniFramework.Runtime.UMEntrance
{
    public class UMiniConfig
    {
        /// <summary>
        /// 启动成功的回调
        /// </summary>
        public Action OnLaunchFinished { get; set; }

        /// <summary>
        /// 框架启动进度
        /// </summary>
        public Action<string, float> LaunchProgress { get; set; }

        /// <summary>
        /// 资源加载器
        /// </summary>
        public IUMAssetLoader AssetLoader { get; set; }

        /// <summary>
        /// 是否在控制台显示持久化数据
        /// </summary>
        public bool IsPersiDataToConsole { get; set; }

        /// <summary>
        /// 是否在控制台输出UMini相关的log
        /// </summary>
        public bool IsLog { get; set; }

        /// <summary>
        /// 配置表
        /// </summary>
        public List<UMConfigTable> ConfigTableList { get; set; }

        /// <summary>
        /// 持久化数据
        /// </summary>
        public List<UMPersistentData> PersistentDataList { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public List<string> EventTypeList { get; set; }
    }
}