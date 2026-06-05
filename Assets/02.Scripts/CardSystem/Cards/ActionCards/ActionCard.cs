using System;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards
{
    public abstract class ActionCard : AbstractCard
    {
        public int AttackValue
        {
            get => _attackValue;
            private set
            {
                _attackValue = value;
                OnAttackValueChanged?.Invoke(_attackValue);
            }
        }
        public int DefenseValue
        {
            get => _defenseValue;
            private set
            {
                _defenseValue = value; 
                OnDefenseValueChanged?.Invoke(_defenseValue);
            }
        }

        [field: SerializeField] public ActionCardDataSO ActionCardData { get; private set; }
        public event Action<int> OnAttackValueChanged;
        public event Action<int> OnDefenseValueChanged;

        [SerializeField] protected EventChannelSO playerChannel;
        [SerializeField] protected EventChannelSO enemyChannel;

        private int _attackValue = 0;
        private int _defenseValue = 0;

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            OnAttackValueChanged?.Invoke(_attackValue);
            OnDefenseValueChanged?.Invoke(_defenseValue);
        }

        public void ChangeValue(int value)
        {
            if (ActionCardData.ActionCardType == ActionCardType.Attack)
                AttackValue += value;
            else if (ActionCardData.ActionCardType == ActionCardType.Defense)
                DefenseValue += value;
        }

        public void AddAttackValue(int value) => AttackValue += value;
        public void AddDefenseValue(int value) => DefenseValue += value;

        public void TakeDamage(int value)
        {
            int overflow = value - DefenseValue;
            DefenseValue = Mathf.Max(DefenseValue - value, 0);
            if (overflow > 0)
            {
                if (this is PlayerActionCard)
                {
                    playerChannel.RaiseEvent(new TakeDamageEvent().Init(overflow));
                }
                else if (this is EnemyActionCard)
                {
                    enemyChannel.RaiseEvent(new TakeDamageEvent().Init(overflow));
                }
            }
        }
    }
}
