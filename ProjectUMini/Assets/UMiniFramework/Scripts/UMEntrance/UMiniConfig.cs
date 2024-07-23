using System;
using System.Collections.Generic;
using UMiniFramework.Scripts.Modules.AssetModule.AssetLoaders;
using UMiniFramework.Scripts.Modules.ConfigModule;
using UMiniFramework.Scripts.Modules.PersistentDataModule;

namespace UMiniFramework.Scripts.UMEntrance
{
    public class UMiniConfig
    {
        /// <summary>
        /// 启动成功的回调
        /// </summary>
        public Action OnLaunchFinished { get; set; }

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
        /// 消息时间
        /// </summary>
        public List<string> MessageEventList { get; set; }
    }
}