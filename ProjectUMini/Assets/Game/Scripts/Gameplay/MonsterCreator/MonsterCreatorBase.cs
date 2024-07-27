using UMiniFramework.Scripts.UMEntrance;
using UMiniFramework.Scripts.Pool.GameObjectPool;
using UnityEngine;

namespace Game.Scripts.Gameplay.MonsterCreator
{
    public abstract class MonsterCreatorBase : MonoBehaviour
    {
        protected MonsterData m_monsterData;
        protected UMGameObjectPool m_monsterPool;

        public virtual void Init(MonsterData data)
        {
            m_monsterData = data;
            UMini.Asset.LoadAsync<GameObject>(data.monsterPath,
                (res) =>
                {
                    m_monsterPool =
                        UMGameObjectPool.CreatePool(new UMGameObjectPool.UMPoolConfig(
                            $"MonsterType-[{m_monsterData.type}]",
                            gameObject,
                            Instantiate(res.Resource),
                            1,
                            null,
                            null
                        ));
                });
        }

        public abstract void Create();
        public abstract void Stop();

        public abstract int CreateType();
    }
}