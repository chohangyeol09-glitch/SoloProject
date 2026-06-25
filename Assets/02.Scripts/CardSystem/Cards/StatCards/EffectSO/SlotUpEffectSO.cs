using _02.Scripts.CardSystem.Cards.ActionCards;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    [CreateAssetMenu(fileName = "SlotUp", menuName = "Card/Stat/Effect/SlotUp", order = 0)]
    public class SlotUpEffectSO : AbstractStatEffectSO
    {
        [SerializeField] private int slotNum;
        [SerializeField] private GradeValue addValue;

        // {0}=칸 번호(1-based), {1}=더하는 값
        public override object[] GetDescriptionArgs(CardGrade grade) => new object[] { slotNum + 1, addValue.Get(grade) };

        public override bool IsActivate(StatExecuteContext context)
        {
            return slotNum == context.TargetActionCard.OriginalSlot.SlotNumber;
        }

        public override void Apply(StatExecuteContext context)
        {
            context.TargetActionCard.ChangeValue(addValue.Get(GetCardGrade(context)));
        }
    }
}
