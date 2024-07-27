using UMiniFramework.Scripts.Pool.GameObjectPool;
using UnityEngine;

namespace Game.Scripts.Gameplay.Monster
{
    public class BossMonster : MonsterBase
    {
        public override void OnBorn(UMGameObjectPool monsterPool)
        {
            m_animator.Play("Idle");
        }

        public override void OnDamage(float val)
        {
            m_animator.Play("Damage");
        }

        private void OnDamageOver()
        {
            m_animator.Play("Idle");
        }
    }
}