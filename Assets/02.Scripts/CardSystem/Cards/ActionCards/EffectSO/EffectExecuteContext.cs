using System.Collections.Generic;
using _02.Scripts.SlotSystem;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    public class EffectExecuteContext
    {
        public ActionCard ActionCard {get; private set;}
        public List<AbstractSlot> TargetSlots {get; private set;}

        public EffectExecuteContext(ActionCard actionCard, List<AbstractSlot> targets)
        {
            ActionCard = actionCard;
            TargetSlots = targets;
        }
        
    }
}