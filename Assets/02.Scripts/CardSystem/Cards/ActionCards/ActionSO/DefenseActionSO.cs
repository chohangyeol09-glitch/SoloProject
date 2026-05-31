using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.ActionSO
{
    [CreateAssetMenu(fileName = "DefenseAction", menuName = "Card/Action/DefenseAction")]
    public class DefenseActionSO : AbstractActionSO
    {
        public override void Execute(ActionCard card, List<AbstractSlot> targets)
        {
            foreach (AbstractSlot target in targets)
            {
                target.CurrentCard?.AddValue(card.Value);
            }
        }
    }
}