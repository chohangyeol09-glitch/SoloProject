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
                string log = "Attack, Damage: " + card.AttackValue + " Targets: ";
                foreach (AbstractSlot slot in targets)
                {
                    log += slot.SlotNumber + ", ";
                }
                Debug.Log(log); //공격 되는지 보기
                target.CurrentCard?.TakeDamage(card.AttackValue);
            }
        }
    }
}