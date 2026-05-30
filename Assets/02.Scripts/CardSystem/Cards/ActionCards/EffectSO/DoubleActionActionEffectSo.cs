using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "DoubleAction",menuName = "Card/Action/Effect/DoubleAction")]
    public class DoubleActionActionEffectSo : AbstractActionEffectSO
    {
        [field: SerializeField] public int TargetValue {get; private set;}
        
        public override bool IsActivate(EffectExecuteContext context) => context.ActionCard.Value == TargetValue;

        public override void Apply(EffectExecuteContext context, List<Slot> targets)
        {
            context.ActionCard.ActionCadeData.Action.Execute(context.ActionCard, targets);
        }
    }
}