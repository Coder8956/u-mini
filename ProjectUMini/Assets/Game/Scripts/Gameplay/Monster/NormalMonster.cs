using System;
using UMiniFramework.Scripts.Pool.GameObjectPool;
using UMiniFramework.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Gameplay.Monster
{
    public class NormalMonster : MonsterBase
    {
        private bool isWalk = false;
        private Animator m_animator;
        private GameObjectPool m_pool;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        private void Walk()
        {
            isWalk = true;
            m_animator.Play("Walk");
        }

        public void StopWalk()
        {
            isWalk = false;
        }

        private void FixedUpdate()
        {
            Walking();
        }

        private void Walking()
        {
            if (!isWalk) return;
            transform.Translate(Vector3.forward * 1 * Time.fixedDeltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "HumanHome")
            {
                StopWalk();
                m_animator.Play("Attack");
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.tag == "HumanHome")
            {
                StopWalk();
                m_pool.Back(gameObject);
            }
        }

        private void OnAttackOver()
        {
            Walk();
        }

        public void Idle(GameObjectPool monsterPool)
        {
            m_pool = monsterPool;
            Walk();
        }
    }
}