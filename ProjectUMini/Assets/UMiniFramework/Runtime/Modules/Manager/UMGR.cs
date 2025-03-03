using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Manager.Info;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Manager
{
    public class UMGR : MonoBehaviour
    {
        /// <summary>
        /// 全局启动标记
        /// </summary>
        private static bool Global_Launched = false;

        private static UMGR UMGR_Instance;

        private const string UMGR_GO_NAME = "UMini-UMGR";

        private static GameObject UMGR_GameObject = null;

        private static Dictionary<string, ModuleRegisterInfo> ModuleDic = null;

        private static FieldInfo Field_IPI_InitState;
        private static FieldInfo Field_IPI_InitModule;
        private static FieldInfo Field_IPI_InitProgress;
        
        private IEnumerator InitModulesCoro(Action<InitProgressInfo> initCallback)
        {
            Type IPIType = typeof(InitProgressInfo);
            Field_IPI_InitState = UMUtilCommon.GetObjectNoPublicField(IPIType, "m_initState");
            Field_IPI_InitModule = UMUtilCommon.GetObjectNoPublicField(IPIType, "m_initModule");
            Field_IPI_InitProgress = UMUtilCommon.GetObjectNoPublicField(IPIType, "m_initProgress");

            InitProgressInfo initInfo = new InitProgressInfo();
            Field_IPI_InitState.SetValue(initInfo, false);
            float moduleCount = ModuleDic.Count;
            int initedNum = 0;
            MethodInfo ModuleInitMethod = null;

            foreach (var ele in ModuleDic)
            {
                ModuleRegisterInfo registerInfo = ele.Value;
                Field_IPI_InitModule.SetValue(initInfo, registerInfo.Module);
                Field_IPI_InitProgress.SetValue(initInfo, (initedNum / moduleCount));
                initCallback?.Invoke(initInfo);
                Type moduleType = registerInfo.Module.GetType();
                ModuleInitMethod = UMUtilCommon.GetObjectNoPublicMethod(moduleType, "Init");
                yield return ModuleInitMethod.Invoke(registerInfo.Module, new object[] {registerInfo.InitArgs});
                initedNum++;
            }

            Field_IPI_InitModule.SetValue(initInfo, null);
            Field_IPI_InitProgress.SetValue(initInfo, (initedNum / moduleCount));
            Field_IPI_InitState.SetValue(initInfo, true);
            initCallback?.Invoke(initInfo);
        }

        private static string GetModuleKey<T>() where T : UMBaseModule
        {
            return typeof(T).Name;
        }

        public static void Launch()
        {
            if (Global_Launched) return;
            UMGR_Instance = UMUtilCommon.CreateGameObject<UMGR>(UMGR_GO_NAME, null);
            UMGR_GameObject = UMGR_Instance.gameObject;
            DontDestroyOnLoad(UMGR_GameObject);
            ModuleDic = new Dictionary<string, ModuleRegisterInfo>();
            Global_Launched = true;
            UMUtilDebug.Log($"UMGR Launched.");
        }

        public static void Register<T>(UMModuleInitArgs initArgs = null) where T : UMBaseModule
        {
            string key = GetModuleKey<T>();
            if (ModuleDic.ContainsKey(key))
            {
                UMUtilDebug.Warning($"Incorrect operation. The {key} was registered repeatedly.");
            }
            else
            {
                T module = UMUtilCommon.CreateGameObject<T>(key, UMGR_GameObject);
                ModuleRegisterInfo registerInfo = new ModuleRegisterInfo(module, initArgs);
                ModuleDic.Add(key, registerInfo);
                UMUtilDebug.Log($"UMGR register module: {key}.");
            }
        }

        public static void InitModules(Action<InitProgressInfo> initCallback)
        {
            UMGR_Instance.StartCoroutine(UMGR_Instance.InitModulesCoro(initCallback));
        }

        public static T Get<T>() where T : UMBaseModule
        {
            string key = GetModuleKey<T>();
            if (ModuleDic.ContainsKey(key))
            {
                return ModuleDic[key].Module as T;
            }

            UMUtilDebug.Warning($"UMGR The {key} module is not registered");

            return null;
        }
    }
}