using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.InteractionSystemSystem;
using _02.Scripts.InteractionSystemSystem.Interactions;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards
{
    public class ActionCard : AbstractCard, IDropTarget
    {
        public int Value { get; private set; }
        public DropInteraction DropInteraction { get; private set; }
        [field: SerializeField] public ActionCadeDataSO ActionCadeData { get; private set; }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            DropInteraction = GetModule<DropInteraction>();
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            DropInteraction.OnDropped += OnDrop;
        }

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

            bool isUse = PlayerManager.Instance.ChangeCost(statCard.NeedCost);
            if (!isUse) return;

            AddValue(statCard.StatData.Value);
            statCard.OnUsed(); 
        }
    }
}