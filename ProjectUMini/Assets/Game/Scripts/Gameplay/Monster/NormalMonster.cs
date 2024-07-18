using UMiniFramework.Scripts.Pool.GameObjectPool;
using UnityEngine;

namespace Game.Scripts.Gameplay.Monster
{
    public class NormalMonster : MonsterBase
    {
        [SerializeField] private float m_speed = 2;
        private bool m_isWalk = false;
        private GameObjectPool m_pool;
        private float m_hp = 3;

        private void Walk()
        {
            m_isWalk = true;
            m_animator.Play("Walk");
        }

        public void StopWalk()
        {
            m_isWalk = false;
        }

        private void FixedUpdate()
        {
            Walking();
        }

        private void Walking()
        {
            if (!m_isWalk) return;
            transform.Translate(Vector3.forward * m_speed * Time.fixedDeltaTime);
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

        public override void OnBorn(GameObjectPool monsterPool)
        {
            m_pool = monsterPool;
            m_hp = 3;
            m_collider.enabled = true;
            Walk();
        }


        public override void OnDamage(float val)
        {
            m_hp -= val;
            StopWalk();
            if (m_hp <= 0)
            {
                m_collider.enabled = false;
                m_animator.Play("Death");
            }
            else
            {
                m_animator.Play("Damage");
            }
        }

        private void OnDamageOver()
        {
            Walk();
        }

        private void OnDeathOver()
        {
            m_pool.Back(gameObject);
        }
    }
}