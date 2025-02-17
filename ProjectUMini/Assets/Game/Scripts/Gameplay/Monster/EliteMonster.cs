﻿using UMiniFramework.Runtime.Pool.GameObjectPool;

namespace Game.Scripts.Gameplay.Monster
{
    public class EliteMonster : MonsterBase
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