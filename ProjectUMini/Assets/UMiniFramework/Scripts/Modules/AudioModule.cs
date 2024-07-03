using System.Collections;
using UMiniFramework.Scripts.Function.Audio;
using UMiniFramework.Scripts.Kit;

namespace UMiniFramework.Scripts.Modules
{
    public class AudioModule : UMModule
    {
        public BGMFunc BGM { get; private set; }
        public EffectFunc Effect { get; private set; }

        public override IEnumerator Init()
        {
            BGM = UMTool.CreateGameObject<BGMFunc>(gameObject);
            BGM.Init();

            Effect = UMTool.CreateGameObject<EffectFunc>(gameObject);
            Effect.Init();
            yield return null;
        }
    }
}