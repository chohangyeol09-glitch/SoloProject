using System.Collections.Generic;
using _02.Scripts.SlotSystem;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    public class EffectExecuteContext
    {
        public ActionCard ActionCard {get; private set;}
        public List<AbstractSlot> TargetSlots {get; private set;}
        public AbstractSlot SourceSlot {get; private set;}
        public CardGrade Grade {get; private set;}

        public EffectExecuteContext(ActionCard actionCard, List<AbstractSlot> targets, AbstractSlot sourceSlot = null, CardGrade grade = default)
        {
            ActionCard = actionCard;
            TargetSlots = targets;
            SourceSlot = sourceSlot;
            Grade = grade;
        }
    }
}
