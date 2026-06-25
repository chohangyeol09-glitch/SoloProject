using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "SlotUpEffect", menuName = "Card/Action/Effect/SlotUpEffect")]
    public class SlotUpActionEffectSO : AbstractActionEffectSO
    {
        [SerializeField] private int slotNumber;
        [SerializeField] private GradeValue addValue;

        public override int GetDisplayValue(CardGrade grade) => addValue.Get(grade);

        // {0}=칸 번호(1-based, n번째), {1}=더하는 값(등급별)
        public override string GetDescription(CardGrade grade)
            => string.Format(Description, slotNumber + 1, addValue.Get(grade));

        public override bool IsActivate(EffectExecuteContext context)
            => context.SourceSlot != null && context.SourceSlot.SlotNumber == slotNumber;

        public override UniTask Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            context.ActionCard.ChangeValue(addValue.Get(context.Grade));
            return UniTask.CompletedTask;
        }
    }
}
