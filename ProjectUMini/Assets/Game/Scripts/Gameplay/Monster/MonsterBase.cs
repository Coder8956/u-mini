using UMiniFramework.Scripts.Pool.GameObjectPool;
using UnityEngine;

namespace Game.Scripts.Gameplay.Monster
{
    public abstract class MonsterBase : MonoBehaviour
    {
        public abstract void OnBorn(GameObjectPool monsterPool);
        public abstract void OnDamage(float val);
    }
}