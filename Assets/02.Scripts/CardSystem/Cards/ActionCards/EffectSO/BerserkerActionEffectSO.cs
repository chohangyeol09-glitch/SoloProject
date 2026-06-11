using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "BerserkerAction",menuName = "Card/Action/Effect/BerserkerAction")]
    public class BerserkerActionEffectSO : AbstractActionEffectSO
    {
        [SerializeField] private int targetHealth;
        
        public override bool IsActivate(EffectExecuteContext context) => Players.Player.Instance.CurrentHealth <= targetHealth;
        public override void Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            context.ActionCard.AddAttackValue(5);
        }
    }
}