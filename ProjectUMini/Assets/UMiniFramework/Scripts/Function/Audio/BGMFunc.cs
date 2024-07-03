using UnityEngine;

namespace UMiniFramework.Scripts.Function.Audio
{
    public class BGMFunc : AudioFunc
    {
        public override void Init()
        {
            gameObject.AddComponent<AudioSource>();
        }
    }
}