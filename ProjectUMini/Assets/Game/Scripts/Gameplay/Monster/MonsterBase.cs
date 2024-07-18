using UMiniFramework.Scripts.Pool.GameObjectPool;
using UnityEngine;

namespace Game.Scripts.Gameplay.Monster
{
    public abstract class MonsterBase : MonoBehaviour
    {
        protected CapsuleCollider m_collider;
        protected Animator m_animator;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_collider = GetComponent<CapsuleCollider>();
        }

        public abstract void OnBorn(GameObjectPool monsterPool);
        public abstract void OnDamage(float val);
    }
}