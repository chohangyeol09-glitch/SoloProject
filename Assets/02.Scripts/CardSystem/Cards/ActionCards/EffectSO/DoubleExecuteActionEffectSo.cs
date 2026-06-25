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
        [SerializeField] private GradeValue repeatCount;

        // {0}=반복횟수, {1}=발동 공격값
        public override int GetDisplayValue(CardGrade grade) => repeatCount.Get(grade);
        public override string GetDescription(CardGrade grade) => string.Format(Description, GetDisplayValue(grade), TargetValue);

        public override bool IsActivate(EffectExecuteContext context)
            => context.ActionCard.AttackValue == TargetValue;

        public override async UniTask Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            int count = repeatCount.Get(context.Grade);
            for (int i = 0; i < count; i++)
                await context.ActionCard.ActionCardData.Action.Execute(context.ActionCard, targets);
        }
    }
}
