using System.Collections;
using Game.Scripts.Gameplay.Monster;
using UnityEngine;

namespace Game.Scripts.Gameplay.MonsterCreator
{
    public class NormalMonsterCreator : MonsterCreatorBase
    {
        private Coroutine m_createMonsterCoro;
        private WaitForSeconds m_createIntervalTime = new WaitForSeconds(2f);

        public override void Create()
        {
            m_createMonsterCoro = StartCoroutine(CreateNormalMonster());
        }

        private IEnumerator CreateNormalMonster()
        {
            yield return new WaitUntil(() => { return m_monsterPool != null; });
            while (true)
            {
                NormalMonster monster = m_monsterPool.Get().GetComponent<NormalMonster>();
                monster.transform.SetParent(null);
                monster.transform.position = transform.position;
                monster.transform.rotation = transform.rotation;
                monster.Idle(m_monsterPool);
                yield return m_createIntervalTime;
            }
        }

        public override void Stop()
        {
        }

        public override int CreateType()
        {
            return 0;
        }
    }
}