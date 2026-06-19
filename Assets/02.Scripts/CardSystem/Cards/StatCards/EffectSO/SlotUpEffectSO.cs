using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    [CreateAssetMenu(fileName = "SlotUp", menuName = "Card/Stat/Effect/SlotUp", order = 0)]
    public class SlotUpEffectSO : AbstractStatEffectSO
    {
        [SerializeField] private int slotNum;
        [SerializeField] private int addValue;
        
        public override bool IsActivate(StatExecuteContext context)
        {
            return slotNum == context.TargetActionCard.OriginalSlot.SlotNumber;
        }

        public override void Apply(StatExecuteContext context)
        {
            if (addValueType == ActionCardType.Attack)
                context.TargetActionCard.AddAttackValue(addValue);
            else
                context.TargetActionCard.AddDefenseValue(addValue);
        }
    }
}