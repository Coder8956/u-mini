using System.Collections;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Audio.BGM;
using UMiniFramework.Runtime.Modules.Audio.Effect;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Utils;

namespace UMiniFramework.Runtime.Modules.Audio
{
    public class UMAudio : UMBaseModule
    {
        private const string BGM_GO_NAME = "BGM_UMAUDIO";
        private const string EFFECT_GO_NAME = "EFFECT_UMAUDIO";
        private UMAudioInitArgs m_initArgs = null;
        public UMAudioBGM BGM { get; private set; }
        public UMAudioEffect Effect { get; private set; }

        public override UMModuleType ModuleType
        {
            get => UMModuleType.Audio;
        }

        protected override IEnumerator Init(UMModuleInitArgs initArgs)
        {
            m_initArgs = UMUtilCommon.ConvertObjectClass<UMAudioInitArgs>(initArgs);

            // 初始化 BGM
            BGM = UMUtilCommon.CreateGameObject<UMAudioBGM>(BGM_GO_NAME, gameObject);
            MethodInfo BGMInit = UMUtilCommon.GetObjectNoPublicMethod(BGM.GetType(), "Init");
            BGMInit.Invoke(BGM, new object[] {m_initArgs});

            // 初始化 Effect
            Effect = UMUtilCommon.CreateGameObject<UMAudioEffect>(EFFECT_GO_NAME, gameObject);
            MethodInfo EffectInit = UMUtilCommon.GetObjectNoPublicMethod(Effect.GetType(), "Init");
            EffectInit.Invoke(Effect, new object[] {m_initArgs});

            yield return null;
        }
    }
}