using System.Collections;
using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.Audio
{
    public class UMAudio : UMBaseModule
    {
        public UMBGMAudio BGM { get; private set; }
        public UMEffectAudio Effect { get; private set; }

        public override IEnumerator Init(UMModuleConfig config)
        {
            yield return null;
            // BGM = UMUtilCommon.CreateGameObject<UMBGMAudio>(nameof(UMBGMAudio), gameObject);
            // BGM.Init();
            //
            // Effect = UMUtilCommon.CreateGameObject<UMEffectAudio>(nameof(UMEffectAudio), gameObject);
            // Effect.Init();
            // yield return null;
            // m_initFinished = true;
            // UMUtilCommon.PrintModuleInitFinishedLog(GetType().Name, m_initFinished);
        }
    }
}