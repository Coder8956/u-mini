using UnityEngine;

namespace Game.Scripts.Gameplay.MonsterCreator
{
    public abstract class MonsterCreatorBase : MonoBehaviour
    {
        protected MonsterData m_monsterData;

        public virtual void Init(MonsterData data)
        {
            m_monsterData = data;
        }

        public abstract void Create();
        public abstract void Stop();
    }
}