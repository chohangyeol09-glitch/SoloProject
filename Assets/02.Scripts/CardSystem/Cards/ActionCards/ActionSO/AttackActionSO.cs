using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.ActionSO
{
    [CreateAssetMenu(fileName = "AttackAction", menuName = "Card/Action/AttackAction")]
    public class AttackActionSO : AbstractActionSO
    {
        public override void Execute(ActionCard card, List<AbstractSlot> targets)
        {
            foreach (AbstractSlot target in targets)
            {
                if (target == null)
                    continue;
                Debug.Log("Attack, Damage: " + card.AttackValue + " Targets: " + target.SlotNumber); 
                target.CurrentCard?.TakeDamage(card.AttackValue);
            }
        }
    }
}