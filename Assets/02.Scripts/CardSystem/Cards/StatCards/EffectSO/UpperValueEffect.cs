using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    [CreateAssetMenu(fileName = "UpperValue", menuName = "Card/Stat/Effect/UpperValue", order = 0)]
    public class UpperValueEffect : AbstractStatEffectSO
    {
        [SerializeField] private int upperValue;
        [SerializeField] private GradeValue addValue;

        public override object[] GetDescriptionArgs(CardGrade grade) => new object[] { upperValue, addValue.Get(grade) };

        public override bool IsActivate(StatExecuteContext context)
        {
            return context.TargetActionCard.AttackValue + context.TargetActionCard.DefenseValue > upperValue;
        }

        public override void Apply(StatExecuteContext context)
        {
            context.TargetActionCard.ChangeValue(addValue.Get(GetCardGrade(context)));
        }
    }
}
