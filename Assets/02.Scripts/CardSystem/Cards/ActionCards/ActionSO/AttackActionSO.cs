using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.ActionSO
{
    [CreateAssetMenu(fileName = "AttackAction", menuName = "Card/Action/AttackAction")]
    public class AttackActionSO : AbstractActionSO
    {
        public override void Execute(ActionCard card, List<Slot> targets)
        {
            foreach (Slot target in targets)
            {
                target.CurrentCard?.TakeDamage(card.Value);
            }
        }
    }
}