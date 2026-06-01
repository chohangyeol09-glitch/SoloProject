using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "DoubleAction",menuName = "Card/Action/Effect/DoubleAction")]
    public class DoubleExecuteActionEffectSo : AbstractActionEffectSO
    {
        [field: SerializeField] public int TargetValue {get; private set;}
        
        public override bool IsActivate(EffectExecuteContext context) => context.ActionCard.AttackValue == TargetValue;

        public override void Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            context.ActionCard.ActionCardData.Action.Execute(context.ActionCard, targets);
        }
    }
}