using System.Reflection;
using UMiniFramework.Runtime.Utils;
using UnityEngine;

namespace UMiniFramework.Runtime.Modules.Audio.Base
{
    public abstract class UMAudioFunc : MonoBehaviour
    {
        private MethodInfo m_LoadClipMethod;

        protected abstract void Init(UMAudioInitArgs initArgs);

        protected void LoadClipInACI(AudioClipInfo aci)
        {
            if (m_LoadClipMethod == null)
            {
                m_LoadClipMethod = UMUtilCommon.GetObjectNoPublicMethod(typeof(AudioClipInfo), "LoadClip");
            }

            m_LoadClipMethod.Invoke(aci, null);
        }
    }
}