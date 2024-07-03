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
            BGM = UMModuleTool.CreateGameObject<BGMFunc>(gameObject);
            Effect = UMModuleTool.CreateGameObject<EffectFunc>(gameObject);
            yield return null;
        }
    }
}