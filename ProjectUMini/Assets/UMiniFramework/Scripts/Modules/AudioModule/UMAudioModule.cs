using System.Collections;
using UMiniFramework.Scripts.Kit;

namespace UMiniFramework.Scripts.Modules.AudioModule
{
    public class UMAudioModule : UMModule
    {
        public BGMAudio BGM { get; private set; }
        public EffectAudio Effect { get; private set; }

        public override IEnumerator Init(UMini.UMiniConfig config)
        {
            BGM = UMTool.CreateGameObject<BGMAudio>(nameof(BGMAudio), gameObject);
            BGM.Init();

            Effect = UMTool.CreateGameObject<EffectAudio>(nameof(EffectAudio), gameObject);
            Effect.Init();
            yield return null;
        }
    }
}