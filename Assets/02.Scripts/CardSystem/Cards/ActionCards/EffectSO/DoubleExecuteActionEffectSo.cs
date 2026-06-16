using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "DoubleEffect", menuName = "Card/Action/Effect/DoubleEffect")]
    public class DoubleExecuteActionEffectSo : AbstractActionEffectSO
    {
        [field: SerializeField] public int TargetValue { get; private set; }
        [field: SerializeField] public int RepeatCount { get; private set; } = 1; 

        public override bool IsActivate(EffectExecuteContext context)
            => context.ActionCard.AttackValue == TargetValue;

        public override async UniTask Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            for (int i = 0; i < RepeatCount; i++)
                await context.ActionCard.ActionCardData.Action.Execute(context.ActionCard, targets);
        }
    }
}