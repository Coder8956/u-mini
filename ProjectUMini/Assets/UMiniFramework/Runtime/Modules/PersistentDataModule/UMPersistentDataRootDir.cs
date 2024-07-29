using System;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.PersistentDataModule
{
    public static class UMPersistentDataRootDir
    {
        public static string GetRootDir()
        {
            return String.Concat(Application.streamingAssetsPath, "/UMPersiData");
        }
    }
}