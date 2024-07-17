using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class GameCannon : MonoBehaviour
    {
        [SerializeField] private GameObject m_cannonDeck;
        public GameObject CannonDeck => m_cannonDeck;

        [SerializeField] private GameObject m_gun;
        public GameObject Gun => m_gun;

        [SerializeField] private GameObject m_shootingPoint;
        public GameObject ShootingPoint => m_shootingPoint;
    }
}