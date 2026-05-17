using System.Collections;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UMiniFramework.Runtime.Modules
{
    public class UMScene : UMBaseModule
    {
        public override UMModuleType ModuleType
        {
            get => UMModuleType.Scene;
        }

        /// <summary>
        /// 开始加载场景
        /// </summary>
        public UnityAction OnLoadStart;

        /// <summary>
        /// 场景正在加载
        /// </summary>
        public UnityAction<float> OnLoading;

        /// <summary>
        /// 完成场景加载
        /// </summary>
        public UnityAction OnLoadCompleted;

        protected override IEnumerator Init()
        {
            UMUtilDebug.Log($"{GetType().Name} Inited");
            yield return null;
        }

        public void Load(string sceneName, bool isAsy = true, float switchDelay = 0.3f)
        {
            StartCoroutine(LoadSceneAsync(sceneName, isAsy, switchDelay));
        }

        private IEnumerator LoadSceneAsync(string sceneName, bool isAsy, float switchDelay)
        {
            OnLoadStart?.Invoke();

            if (isAsy)
            {
                AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
                ao.allowSceneActivation = true;
                while (ao.isDone)
                {
                    OnLoading?.Invoke(ao.progress);
                    // UMUtilDebug.Log($"scene load progress: {ao.progress}");
                    yield return null;
                }
            }
            else
            {
                OnLoadStart?.Invoke();
                SceneManager.LoadScene(sceneName);
                OnLoadCompleted?.Invoke();
            }

            yield return new WaitForSeconds(switchDelay);

            OnLoadCompleted?.Invoke();
        }
    }
}