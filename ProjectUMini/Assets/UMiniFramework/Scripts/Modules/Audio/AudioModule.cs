using System.Collections;
using UMiniFramework.Scripts.Kit;

namespace UMiniFramework.Scripts.Modules.Audio
{
    public class AudioModule : UMModule
    {
        public BGMAudio BGM { get; private set; }
        public EffectAudio Effect { get; private set; }

        public override IEnumerator Init()
        {
            BGM = UMTool.CreateGameObject<BGMAudio>(gameObject, nameof(BGMAudio));
            BGM.Init();

            Effect = UMTool.CreateGameObject<EffectAudio>(gameObject, nameof(EffectAudio));
            Effect.Init();
            yield return null;
        }
    }
}