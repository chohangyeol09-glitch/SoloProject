using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.DragSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards
{
    public class ActionCard : AbstractCard, IDropTarget
    {
        public int Value { get; private set; }
        [field: SerializeField] public ActionCadeDataSO ActionCadeData { get; private set; }

        public void AddValue(int value)
        {
            Value += value;
        }

        public void TakeDamage(int value)
        {
            int overDamage = Value - value;
                
            if (overDamage > 0)
                Debug.Log("피해입음: " + overDamage);
        }

        public void OnDrop(Transform dropTrm)
        {
            if (!dropTrm.TryGetComponent<StatCard>(out StatCard statCard)) return;
            if (PlayerManager.Instance.CurrentCost >= statCard.NeedCost) return;

            var context = new StatExecuteContext
            {
                StatCard = statCard,
                ActionCard = this
            };
        }
    }
}