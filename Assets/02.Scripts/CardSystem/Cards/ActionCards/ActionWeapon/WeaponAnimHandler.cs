using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.ActionCardEvents;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.ActionWeapon
{
    public class WeaponAnimHandler : MonoBehaviour
    {
        [SerializeField] private EventChannelSO enemyChannel;
        private int _id; 

        public void SetId(int id) => _id = id;

        public void OnHit()
        {
            enemyChannel.RaiseEvent(new WeaponHitEvent().Init(_id));
        }
        public void OnAttackEnd()
        {
            enemyChannel.RaiseEvent(new WeaponAttackEndEvent().Init(_id));
        }
    }
}