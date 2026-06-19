using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    [CreateAssetMenu(fileName = "UpperValue", menuName = "Card/Stat/Effect/UpperValue", order = 0)]
    public class UpperValueEffect : AbstractStatEffectSO
    {
        [SerializeField] private int upperValue;
        [SerializeField] private int addValue;
        public override bool IsActivate(StatExecuteContext context)
        {
            return context.TargetActionCard.AttackValue + context.TargetActionCard.DefenseValue > upperValue;
        }

        public override void Apply(StatExecuteContext context)
        {
            if (addValueType == ActionCardType.Attack)
                context.TargetActionCard.AddAttackValue(addValue);
            if  (addValueType == ActionCardType.Defense)
                context.TargetActionCard.AddDefenseValue(addValue);
        }
    }
}