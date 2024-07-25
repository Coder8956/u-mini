using System.Collections;
using UMiniFramework.Scripts.UMEntrance;
using UMiniFramework.Scripts.Utils;

namespace UMiniFramework.Scripts.Modules.AudioModule
{
    public class UMAudioModule : UMModule
    {
        public UMBGMAudio BGM { get; private set; }
        public UMEffectAudio Effect { get; private set; }

        public override IEnumerator Init(UMiniConfig config)
        {
            BGM = UMUtilCommon.CreateGameObject<UMBGMAudio>(nameof(UMBGMAudio), gameObject);
            BGM.Init();

            Effect = UMUtilCommon.CreateGameObject<UMEffectAudio>(nameof(UMEffectAudio), gameObject);
            Effect.Init();
            yield return null;
            m_initFinished = true;
            UMUtilCommon.PrintModuleInitFinishedLog(GetType().Name, m_initFinished);
        }
    }
}