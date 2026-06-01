using System;
using _02.Scripts.CardSystem.Cards.StatCards;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards
{
    public class StatCard : AbstractCard
    {
        [field: SerializeField] public StatCardDataSO StatData { get; private set; }
        private int _needCost = 0;

        public int NeedCost
        {
            get => _needCost;
            set
            {
                _needCost += value;
                _needCost = Mathf.Clamp(_needCost, 0, int.MaxValue);
            }
        }


        protected override void InitializeModules()
        {
            base.InitializeModules();
            CardInteraction.DropLayer = dropLayer;
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
        }

        public void SetStatData(StatCardDataSO statData)
        {
            StatData = statData;
            NeedCost = StatData.Cost;
        }

        public void OnUsed()
        {
            Destroy(gameObject);
        }
    }
}