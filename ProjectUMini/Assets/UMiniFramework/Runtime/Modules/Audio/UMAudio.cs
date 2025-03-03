using System.Collections;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Audio.BGM;
using UMiniFramework.Runtime.Modules.Audio.Effect;
using UMiniFramework.Runtime.Modules.Audio.InitArgs;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Utils;

namespace UMiniFramework.Runtime.Modules.Audio
{
    public class UMAudio : UMBaseModule
    {
        private const string BGM_GO_NAME = "BGM_UMAUDIO";
        private const string EFFECT_GO_NAME = "EFFECT_UMAUDIO";
        private UMAudioInitArgs m_initArgs = null;
        private MethodInfo m_bgmInitMethod = null;
        private MethodInfo m_effectInitMethod = null;

        public UMAudioBGM BGM { get; private set; }
        public UMAudioEffect Effect { get; private set; }

        public override UMModuleType ModuleType
        {
            get => UMModuleType.Audio;
        }

        private void UseDefaultInitArgs()
        {
            m_bgmInitMethod.Invoke(BGM, new object[] {UMAudioDIArgs.BGMAudioClipInfoList()});
            m_effectInitMethod.Invoke(Effect,
                new object[] {UMAudioDIArgs.EffectAudioClipInfoList(), UMAudioDIArgs.EffectAudioDefaultEASCount()});
        }

        private void ReadInitArgs()
        {
            m_bgmInitMethod.Invoke(BGM, new object[] {m_initArgs.BGMClips});
            m_effectInitMethod.Invoke(Effect, new object[] {m_initArgs.EffectClips, m_initArgs.DefaultEASCount});
        }

        protected override IEnumerator Init(UMModuleInitArgs initArgs)
        {
            // 创建 BGM 对象
            BGM = UMUtilCommon.CreateGameObject<UMAudioBGM>(BGM_GO_NAME, gameObject);
            m_bgmInitMethod = UMUtilCommon.GetObjectNoPublicMethod(BGM.GetType(), "InitAudioBGM");

            // 创建 Effect 对象
            Effect = UMUtilCommon.CreateGameObject<UMAudioEffect>(EFFECT_GO_NAME, gameObject);
            m_effectInitMethod = UMUtilCommon.GetObjectNoPublicMethod(Effect.GetType(), "InitAudioEffect");

            m_initArgs = UMUtilCommon.ConvertObjectClass<UMAudioInitArgs>(initArgs);

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
    }
}