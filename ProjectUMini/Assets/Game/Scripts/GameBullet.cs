using UMiniFramework.Runtime.Modules.Audio;
using UMiniFramework.Runtime.Modules.Manager;
using UnityEngine;

namespace Game.Scripts
{
    public class GameBullet : MonoBehaviour
    {
        private BulletData m_data;

        public void SetData(BulletData data)
        {
            m_data = data;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Monster")
            {
                Destroy(other.gameObject);
            }

            UMGR.Get<UMAudio>().Effect.Play(m_data.exploAudioId);
            Destroy(gameObject);
        }
    }
}