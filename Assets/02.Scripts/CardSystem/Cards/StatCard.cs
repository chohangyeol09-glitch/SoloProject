using _02.Scripts.CardSystem.Cards.StatCards;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards
{
    public class StatCard : AbstractCard
    {
        [field: SerializeField] public StatCardDataSO StatData { get; private set; }
        public int NeedCost;

        public override void OnDragEnd()
        {
            base.OnDragEnd();
            Destroy(gameObject);
        }
    }
}