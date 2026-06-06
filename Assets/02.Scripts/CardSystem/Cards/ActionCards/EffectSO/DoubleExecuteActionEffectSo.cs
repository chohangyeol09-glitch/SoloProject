using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "DoubleAction", menuName = "Card/Action/Effect/DoubleAction")]
    public class DoubleExecuteActionEffectSo : AbstractActionEffectSO
    {
        [field: SerializeField] public int TargetValue { get; private set; }
        [field: SerializeField] public int RepeatCount { get; private set; } = 1; // 추가 실행 횟수

        public override bool IsActivate(EffectExecuteContext context)
            => context.ActionCard.AttackValue == TargetValue;

        public override void Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            ExecuteRepeat(context, targets, RepeatCount);
        }

        private void ExecuteRepeat(EffectExecuteContext context, List<AbstractSlot> targets, int remaining)
        {
            if (remaining <= 0) return;

            context.ActionCard.ActionCardData.Action.Execute(
                context.ActionCard,
                targets,
                () => ExecuteRepeat(context, targets, remaining - 1) 
            );
        }
    }
}