using System;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CardSystem.Cards.StatCards.EffectSO;
using _02.Scripts.InteractionSystemSystem;
using _02.Scripts.InteractionSystemSystem.Interactions;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards
{
    public class ActionCard : AbstractCard, IDropTarget
    {
        public int AttackValue
        {
            get => _attackValue;
            set
            {
                _attackValue = value;
                OnAttackValueChanged?.Invoke(_attackValue);
            }
        }
        public int DefenseValue
        {
            get => _defenseValue;
            set
            {
                _defenseValue = value;
                OnAttackValueChanged?.Invoke(_defenseValue);
            }
        }
        public DropInteraction DropInteraction { get; private set; }
        [field: SerializeField] public ActionCardDataSO ActionCardData { get; private set; }
        public event Action<bool> OnDropSuccess;
        public event Action<int> OnAttackValueChanged;
        public event Action<int> OnDefenseValueChanged;

        private int _attackValue = 0;
        private int _defenseValue = 0;
        
        protected override void InitializeModules()
        {
            base.InitializeModules();
            DropInteraction = GetModule<DropInteraction>();
            DropInteraction.OnDropped -= HandleDrop;
            DropInteraction.OnDropped += HandleDrop;
            DropInteraction.SetCanDropType(typeof(StatCard));
        }

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
            if (ActionCardData.ActionCardType == ActionCardType.Defense)
                DefenseValue += value;
        }

        public void TakeDamage(int value)
        {
            Debug.LogWarning(DefenseValue);
            DefenseValue -= value;
            Debug.LogWarning(DefenseValue);
            
            //플레이어 체력 감소
            
            
        }

        public void HandleDrop(Transform dropTrm)
        {
            if (!dropTrm.TryGetComponent<StatCard>(out StatCard statCard))
            {
                OnDropSuccess?.Invoke(false);
                return;
            }
            OnDropSuccess?.Invoke(true);
            
            bool isUse = PlayerManager.Instance.ChangeCost(statCard.NeedCost);
            if (!isUse) return;

            ChangeValue(statCard.StatData.Value);
            if (statCard.StatData.Effects.Count > 0)
            {
                foreach (AbstractStatEffectSO effect in statCard.StatData.Effects)
                {
                    StatExecuteContext context = new StatExecuteContext(statCard, this);
                    
                    if (effect.IsActivate(context))
                        effect.Apply(context);
                }
            }
            statCard.OnUsed(); 
        }
        
        [ContextMenu("kte")]
        private void test() => Debug.LogWarning(DefenseValue);
    }
}
